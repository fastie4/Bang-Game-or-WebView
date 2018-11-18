package com.effortgames.bang.launcher;

import com.effortgames.bang.Constants;
import com.effortgames.bang.api.Api;
import com.effortgames.bang.api.ApiResponse;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class Launcher {
    private Api mApi;
    private Listener mListener;

    public Launcher() {
        mApi = new Retrofit.Builder()
                .addConverterFactory(GsonConverterFactory.create())
                .baseUrl(Constants.BASE_URL)
                .build()
                .create(Api.class);
    }

    public void getAllow() {
        mApi.getAllow().enqueue(mCallback);
    }

    public void setListener(Listener listener) {
        mListener = listener;
    }

    public void destroy() {
        mListener = null;
    }

    public interface Listener {
        void onResult(boolean allow);
        void onFailure();
    }

    private Callback<ApiResponse> mCallback = new Callback<ApiResponse>() {
        @Override
        public void onResponse(Call<ApiResponse> call, Response<ApiResponse> response) {
            if (mListener != null) {
                if (response.body() != null) {
                    mListener.onResult(response.body().getAllow());
                    return;
                }
                mListener.onFailure();
            }
        }

        @Override
        public void onFailure(Call<ApiResponse> call, Throwable t) {
            if (mListener != null) {
                mListener.onFailure();
            }
        }
    };
}
