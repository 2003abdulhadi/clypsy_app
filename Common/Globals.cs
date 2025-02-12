using System;

namespace clypsy.Common
{
  public static class Globals
  {
    public static readonly string apiBaseUrl = Environment.GetEnvironmentVariable("API_BASE_URL") ?? throw new InvalidOperationException("API_BASE_URL is not set in the environment.");
    public static readonly string projectName = Environment.GetEnvironmentVariable("PROJECT_NAME") ?? throw new InvalidOperationException("PROJECT_NAME is not set in the environment.");
  }
}