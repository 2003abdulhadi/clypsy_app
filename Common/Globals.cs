using System;

namespace clypsy.Common
{
  public static class Globals
  {
    public static readonly string apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? throw new InvalidOperationException("API_BASE_URL is not set in the environment.");
  }
}