using CumulioAPI;
using System.Dynamic;
using dotenv.net;
using System;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
DotEnv.Load();
var envVars = DotEnv.Read();

app.MapGet("/", () => {
  Cumulio client = new Cumulio(envVars["CUMUL_KEY"], envVars["CUMUL_TOKEN"], envVars["API_URL"]);
  dynamic properties = new ExpandoObject();
  properties.integration_id = envVars["INTEGRATION_ID"];
  properties.type = "sso";
  properties.expiry = "24 hours";
  properties.inactivity_interval = "10 minutes";
  properties.username = envVars["USER_USERNAME"];
  properties.name = envVars["USER_NAME"];
  properties.email = envVars["USER_EMAIL"];
  properties.suborganization = envVars["USER_SUBORGANIZATION"];
  properties.role = "viewer";

  dynamic authorization = client.create("authorization", properties);
  dynamic authResp = new ExpandoObject();
  authResp.key = authorization.id;
  authResp.token = authorization.token;
  authResp.status = "success";

  return JsonConvert.SerializeObject(authResp);
});

app.Run();
