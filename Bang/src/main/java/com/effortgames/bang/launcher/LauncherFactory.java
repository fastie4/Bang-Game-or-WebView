package com.effortgames.bang.launcher;

public class LauncherFactory {
    private static Launcher sInstance;

    public static Launcher create(Launcher.Listener listener) {
        if (sInstance == null) {
            sInstance = new Launcher();
        }
        sInstance.setListener(listener);
        return sInstance;
    }
}