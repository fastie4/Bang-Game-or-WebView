package com.effortgames.bang;

import android.annotation.SuppressLint;
import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.webkit.WebView;
import android.widget.TextView;
import android.widget.Toast;

import com.effortgames.bang.launcher.Launcher;
import com.effortgames.bang.launcher.LauncherFactory;

@SuppressLint("SetJavaScriptEnabled")
public class LauncherActivity extends Activity implements Launcher.Listener {
    private static final String SAVED_TEXT = "saved_text";
    private Launcher mLauncher;
    private WebView mWebView;
    private TextView mText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_launcher);
        mLauncher = LauncherFactory.create(this);
        mWebView = findViewById(R.id.web_view);
        mText = findViewById(R.id.text);
        if (savedInstanceState == null) {
            SharedPreferences preferences =
                    getSharedPreferences(Constants.PREFERENCES, MODE_PRIVATE);
            int allow = preferences.getInt(Constants.PREFERENCES_ALLOW, Constants.ALLOW_NOT_SET);
            switch (allow) {
                case Constants.ALLOW:
                    openGame();
                    break;
                case Constants.NOT_ALLOW:
                    openUrl();
                    break;
                case Constants.ALLOW_NOT_SET:
                    mLauncher.getAllow();
            }
        } else {
            String text = savedInstanceState.getString(SAVED_TEXT);
            if (text != null) {
                mText.setText(text);
            } else {
                mText.setVisibility(View.GONE);
                mWebView.setVisibility(View.VISIBLE);
                mWebView.restoreState(savedInstanceState);
                mWebView.getSettings().setJavaScriptEnabled(true);
            }
        }
    }

    @Override
    protected void onSaveInstanceState(Bundle outState) {
        if (mText.getVisibility() == View.VISIBLE) {
            outState.putString(SAVED_TEXT, mText.getText().toString());
        } else {
            mWebView.saveState(outState);
        }
        super.onSaveInstanceState(outState);
    }

    @Override
    protected void onDestroy() {
        mLauncher.destroy();
        super.onDestroy();
    }

    @Override
    public void onResult(boolean allow) {
        SharedPreferences preferences = getSharedPreferences(Constants.PREFERENCES, MODE_PRIVATE);
        preferences.edit().putInt(Constants.PREFERENCES_ALLOW, allow? Constants.ALLOW : Constants.NOT_ALLOW).apply();
        if (allow) {
            openGame();
        } else {
            openUrl();
        }
    }

    @Override
    public void onFailure() {
        mText.setText(getString(R.string.error));
    }

    private void openGame() {
        Intent intent = new Intent(LauncherActivity.this, UnityPlayerActivity.class);
        startActivity(intent);
        finish();
    }

    private void openUrl() {
        mText.setVisibility(View.GONE);
        mWebView.setVisibility(View.VISIBLE);
        mWebView.loadUrl(Constants.NOT_ALLOW_URL);
        mWebView.getSettings().setJavaScriptEnabled(true);
    }
}
