﻿namespace shoppingify.IAM.Application;

public interface IAuthenticationProviderService
{
    public Task<string> VerifyToken(string token);
}