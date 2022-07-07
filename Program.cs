using CumulioAPI;
using System.Dynamic;
using dotenv.net;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
DotEnv.Load();
var envVars = DotEnv.Read();

Console.WriteLine(envVars["CUMUL_KEY"]);

app.MapGet("/", () => {
  Cumulio client = new Cumulio(envVars["CUMUL_KEY"], envVars["CUMUL_TOKEN"], envVars["API_URL"], "443");
  dynamic properties = new ExpandoObject();
  properties.integration_id = process.env.INTEGRATION_ID;
  properties.type = "sso";
  properties.expiry = "24 hours";
  properties.inactivity_interval = "1 year";
  properties.username = envVars["USER_USERNAME"];
  properties.name = envVars["USER_NAME"];
  properties.email = envVars["USER_EMAIL"];
  properties.suborganization = envVars["USER_SUBORGANIZATION"];
  properties.role = "viewer";

  dynamic authorization = client.create("authorization", properties);
  dynamic authResp = new ExpandoObject();
  authResp.key = authorization.id;
  authResp.token = authorization.token;

  return Jsonconvert.SerializeObject(authResp);
});

app.Run();
