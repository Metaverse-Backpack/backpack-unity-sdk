mergeInto(LibraryManager.library, {
    InitializeSDK: function (clientId, responseType, url, apiUrl, scriptUrl, state) {
        if (!window.unityInstance) {
          console.error("Please set `window.unityInstance` to your Unity instance before using the Bkpk SDK")
          return
        }
        const script = document.createElement('script')
        script.src = scriptUrl
        
        script.onload = async () => {
          const bkpk = new Bkpk(clientId, { url, apiUrl })
          try {
            const result = await bkpk.authorize(responseType, {
              state,
              scopes: ['avatars:read']
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
