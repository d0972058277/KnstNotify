[![Build Status](https://travis-ci.com/d0972058277/KnstNotify.svg?branch=master)](https://travis-ci.com/d0972058277/KnstNotify)

# KnstNotify
A sender for Apple Push Notification(APN) and Firebase Cloud Message(FCM)

Reference: [CorePush](https://github.com/andrei-m-code/net-core-push-notifications)
***
## Quick Start
### APN
Register in Startup.cs ConfigureServices, for example :
```
services.AddApnConfig(new ApnConfig("{P8-PrivateKey}", "{P8-PrivateKeyId}", "{TeamId}", "{Topic}", ApnServerType.Development));
services.AddKnstNotify();
```
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
apnPayload.Aps.MutableContent = 1;
apnPayload.Aps.ContentAvailable = 1;
```
Send apn payload :
```
IApnSender apnSender = provider.GetRequiredService<IApnSender>();
ApnResult apnResult = await apnSender.SendAsync(apnPayload);
```

### FCM
Register in Startup.cs ConfigureServices, for example :
```
services.AddFcmConfig(new FcmConfig("{ServerKey}"));
// or
services.AddFcmConfig(new FcmConfig("{ServerKey}", "{SenderId}"));
services.AddKnstNotify();
```
Create an fcm payload :
[FcmPayload](https://firebase.google.com/docs/cloud-messaging/http-server-ref.html)
```
FcmPayload fcmPayload = new FcmPayload()
{
    DryRun = true,    // sandbox
    Data = new Dictionary<string, object>()
};
fcmPayload.To = "{deviceToken}";
fcmPayload.Data.Add("title", "title");
fcmPayload.Data.Add("image", "icon");
fcmPayload.Data.Add("message", "message");
fcmPayload.Data.Add("badge", "1");
fcmPayload.Data.Add("sound", "default");
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
IEnumerable<ApnPayload> apnPayloads = tokens.Select(token=>{
    ApnPayload apnPayload = new ApnPayload();
    apnPayload.DeviceToken = token;
    apnPayload.Aps.Badge = 1;
    apnPayload.Aps.Alert.Title = "title";
    apnPayload.Aps.Alert.Subtitle = "subtitle";
    apnPayload.Aps.Alert.Body = "body";
    apnPayload.Aps.Sound = "default";
    apnPayload.Aps.MutableContent = 1;
    apnPayload.Aps.ContentAvailable = 1;
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
        DryRun = true,
        Data = new Dictionary<string, object>()
    };
    fcmPayload.To = token;
    fcmPayload.Data.Add("title", "title");
    fcmPayload.Data.Add("image", "icon");
    fcmPayload.Data.Add("message", "message");
    fcmPayload.Data.Add("badge", "1");
    fcmPayload.Data.Add("sound", "default");
    return fcmPayload;
});
IEnumerable<FcmResult> fcmResults = await fcmSender.SendAsync(fcmPayloads);
```
***
## Support Multi Senders
### APN Example :
```
// In Startup.cs ConfigureServices
services.AddApnConfig(new ApnConfig("{P8-PrivateKey-1}", "{P8-PrivateKeyId-1}", "{TeamId-1}", "{Topic-1}", ApnServerType.Development));
services.AddApnConfig(new ApnConfig("{P8-PrivateKey-2}", "{P8-PrivateKeyId-2}", "{TeamId-2}", "{Topic-2}", ApnServerType.Development));
services.AddKnstNotify();

// Usage
IEnumerable<ApnResult> apnResults = await apnSender.SendAsync(apnPayloads, sender => sender.Configs.First());
```
### FCM Example :
```
// In Startup.cs ConfigureServices
services.AddFcmConfig(new FcmConfig("{ServerKey-1}"));
services.AddFcmConfig(new FcmConfig("{ServerKey-2}"));
services.AddKnstNotify();

// Usage
IEnumerable<FcmResult> fcmResults = await fcmSender.SendAsync(fcmPayloads, sender => sender.Configs.First());
```
