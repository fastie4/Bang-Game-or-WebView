package com.effortgames.bang.api;

import retrofit2.Call;
import retrofit2.http.GET;

public interface Api {
    @GET("/prod")
    Call<ApiResponse> getAllow();
}
