[![Build Status](https://dev.azure.com/KingnetTW/KnstNotify/_apis/build/status/d0972058277.KnstNotify?branchName=master)](https://dev.azure.com/KingnetTW/KnstNotify/_build/latest?definitionId=2&branchName=master)
[![Board Status](https://dev.azure.com/KingnetTW/3aa837e8-1ad5-4957-89e3-22a84b469ad1/11fe6816-3024-4d74-8f5d-f7ffa1d12e9c/_apis/work/boardbadge/2ea5de1b-2543-4a91-b32d-2d976b61d48f?columnOptions=1)](https://dev.azure.com/KingnetTW/3aa837e8-1ad5-4957-89e3-22a84b469ad1/_boards/board/t/11fe6816-3024-4d74-8f5d-f7ffa1d12e9c/Microsoft.RequirementCategory/)
![Nuget](https://img.shields.io/nuget/v/KnstNotify.Core)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/KnstNotify.Core)

![ICON](https://raw.githubusercontent.com/d0972058277/KnstNotify/master/icon.png)
# KnstNotify
A sender for Apple Push Notification(APN) and Firebase Cloud Message(FCM).

Reference: [CorePush](https://github.com/andrei-m-code/net-core-push-notifications)
***
## Quick Start
### APN
Register in Startup.cs ConfigureServices, for example :
```
services.AddApnConfig(new ApnConfig("{P8-PrivateKey}", "{P8-PrivateKeyId}", "{TeamId}", "{Topic}", ApnServerType.Development));
services.AddKnstNotify();
```
P8-PrivateKey(without newline) :
![P8_PrivateKey](https://raw.githubusercontent.com/d0972058277/KnstNotify/master/P8_PrivateKey.PNG)

Create an apn payload :
[ApnPayload](https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification)
```
ApnPayload apnPayload = new ApnPayload();
apnPayload.DeviceToken = "{deviceToken}";
apnPayload.Aps.Badge = 1;
apnPayload.Aps.Alert.Title = "title";
apnPayload.Aps.Alert.Subtitle = "subtitle";
apnPayload.Aps.Alert.Body = "body";
apnPayload.Aps.Sound = "default";
apnPayload.Aps.Add("Key1", "Value1");
apnPayload.Aps["Key2"] = "Value2";
```
Send apn payload :
```
IApnSender apnSender = provider.GetRequiredService<IApnSender>();
ApnResult apnResult = await apnSender.SendAsync(apnPayload);
```

### FCM
Register in Startup.cs ConfigureServices, for example :
```
services.AddFcmConfig(new FcmConfig("{ServerKey}", HostEnvironment.IsDevelopment()));
// or
services.AddFcmConfig(new FcmConfig("{ServerKey}", "{SenderId}", HostEnvironment.IsDevelopment()));
services.AddKnstNotify();
```
Create an fcm payload :
[FcmPayload](https://firebase.google.com/docs/cloud-messaging/http-server-ref.html)
```
FcmPayload fcmPayload = new FcmPayload()
{
    Data = new Dictionary<string, object>(),
    Notification = new Dictionary<string, object>()
};
fcmPayload.To = "{deviceToken}";
fcmPayload.Notification.Add("title", "title");
fcmPayload.Notification.Add("body", "body");
fcmPayload.Data.Add("Key1", "Value1");
fcmPayload.Data["Key2"] = "Value2";
```
Send fcm payload :
```
IFcmSender fcmSender = provider.GetRequiredService<IFcmSender>();
FcmResult fcmResult = await fcmSender.SendAsync(fcmPayload);
```
***
## Multi Notifications
100 tokens for example :
```
IEnumerable<string> tokens = new string[100];
```
### APN
```
IEnumerable<ApnPayload> apnPayloads = tokens.Select(token => {
    ApnPayload apnPayload = new ApnPayload();
    apnPayload.DeviceToken = token;
    apnPayload.Aps.Badge = 1;
    apnPayload.Aps.Alert.Title = "title";
    apnPayload.Aps.Alert.Subtitle = "subtitle";
    apnPayload.Aps.Alert.Body = "body";
    apnPayload.Aps.Sound = "default";
    apnPayload.Aps.Add("Key1", "Value1");
    apnPayload.Aps["Key2"] = "Value2";
    return apnPayload;
});
IEnumerable<ApnResult> apnResults = await apnSender.SendAsync(apnPayloads);
```
### FCM
```
IEnumerable<FcmPayload> fcmPayloads = tokens.Select(token =>
{
    FcmPayload fcmPayload = new FcmPayload()
    {
        Data = new Dictionary<string, object>(),
        Notification = new Dictionary<string, object>()
    };
    fcmPayload.To = token;
    fcmPayload.Notification.Add("title", "title");
    fcmPayload.Notification.Add("body", "body");
    fcmPayload.Data.Add("Key1", "Value1");
    fcmPayload.Data["Key2"] = "Value2";
    return fcmPayload;
});
IEnumerable<FcmResult> fcmResults = await fcmSender.SendAsync(fcmPayloads);
```
***
## Support Multi Configs
### APN Example :
```
// In Startup.cs ConfigureServices
services.AddApnConfig(new ApnConfig("{P8-PrivateKey-1}", "{P8-PrivateKeyId-1}", "{TeamId-1}", "{Topic-1}", HostEnvironment.IsDevelopment() ? ApnServerType.Development : ApnServerType.Production){ Name = "Config1" });
services.AddApnConfig(new ApnConfig("{P8-PrivateKey-2}", "{P8-PrivateKeyId-2}", "{TeamId-2}", "{Topic-2}", HostEnvironment.IsDevelopment() ? ApnServerType.Development : ApnServerType.Production){ Name = "Config2" });
services.AddKnstNotify();

// Usage
IEnumerable<ApnResult> apnResults = await apnSender.SendAsync(apnPayloads, sender => sender.Configs.Single(x => x.Name == "Config1"));
```
### FCM Example :
```
// In Startup.cs ConfigureServices
services.AddFcmConfig(new FcmConfig("{ServerKey-1}"){ Name = "Config1" }, HostEnvironment.IsDevelopment());
services.AddFcmConfig(new FcmConfig("{ServerKey-2}"){ Name = "Config2" }, HostEnvironment.IsDevelopment());
services.AddKnstNotify();

// Usage
IEnumerable<FcmResult> fcmResults = await fcmSender.SendAsync(fcmPayloads, sender => sender.Configs.Single(x => x.Name == "Config1"));
```
