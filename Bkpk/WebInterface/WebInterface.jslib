mergeInto(LibraryManager.library, {
    InitializeSDK: function (clientId, responseType, url, state) {
        if (!window.unityInstance) {
          console.error("Please set `window.unityInstance` to your Unity instance before using the Bkpk SDK")
          return
        }
        const script = document.createElement('script')
        script.src = "https://cdn.jsdelivr.net/npm/@bkpk/sdk/dist/index.js"
        
        script.onload = async () => {
          const bkpk = new Bkpk({ clientId, url })
          try {
            const result = await bkpk.authorize({
              state,
              scopes: ['avatars:read']
              responseType,
            })
            if (responseType === "token") {
              window.unityInstance.SendMessage("BkpkWebInterface", "OnAccessToken", result.token)
            } else if (responseType === "code") {
              window.unityInstance.SendMessage("BkpkWebInterface", "OnAuthorizationCode", result.code)
            }
          } catch (error) {
            window.unityInstance.SendMessage("BkpkWebInterface", "OnUserRejected")
          }
        }
    }
});
