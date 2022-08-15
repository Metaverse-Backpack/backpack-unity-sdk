## API

### Configuration

```csharp
class MyClass : MonoBehaviour {
  Init()
  {
    Bkpk.ClientID = "XXX";
    Bkpk.ResponseType = "token|code";
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

  Init()
  {
    string code = Bkpk.Auth.GetActivationCode(OnAuthorized)
  }
}
```

### Web Authentication
```csharp
class MyClass : MonoBehaviour {
  ...

  Init()
  {
    string code = Bkpk.Auth.RequestAuthorization(OnAuthorized)
  }
}
```

### Manage Avatars

```csharp
PaginatedResponse<AvatarMetadata> avatars = Bkpk.GetAvatars(1);

AvatarMetadata avatarMetadata = Bkpk.GetDefaultAvatar();

Avatar avatar = Bkpk.Avatars.LoadAvatar(avatarMetadata);
```