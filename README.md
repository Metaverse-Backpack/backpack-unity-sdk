## API

### Configuration

```csharp
class MyClass : MonoBehaviour {
  Init()
  {
    // Required
    Bkpk.ClientID = "XXX";
    Bkpk.ResponseType = "token|code";
    
    // Optional
    Bkpk.BkpkUrl = "https://bkpk.io";
    Bkpk.BkpkApiUri = "https://api.bkpk.io";
    Bkpk.WebSdkUrl = "https://jsdelivr.com/...";
  }
}
```

### Native Authentication

```csharp
class MyClass : MonoBehaviour {

  // Use this if using the `code` response type
  void OnAuthorized(Bkpk.AuthorizationCodeResponse response)
  {
    // Send these to your backend to exchange them for
    // an access token using your client secret
    Debug.Log(response.code);
    Debug.Log(response.state);
  }

  async Init()
  {
    string code = await Bkpk.Auth.GetActivationCode(OnAuthorized)
  }
}
```

### Web Authentication
```csharp
class MyClass : MonoBehaviour {
  ...

  async Init()
  {
    string code = await Bkpk.Auth.RequestAuthorization(OnAuthorized)
  }
}
```

### Manage Avatars

```csharp
AvatarInfo[] avatars = await Bkpk.Avatars.GetAvatars();

Bkpk.AvatarInfo avatarInfo = await Bkpk.GetDefaultAvatar();

Bkpk.BkpkAvatar avatar = await Bkpk.Avatars.LoadAvatar(avatarInfo);
```