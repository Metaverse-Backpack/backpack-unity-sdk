mergeInto(LibraryManager.library, {
    InitializeSDK: function (clientId, responseType, url, apiUrl, scriptUrl, state) {
        clientId = UTF8ToString(clientId)
        responseType = UTF8ToString(responseType)
        url = UTF8ToString(url)
        apiUrl = UTF8ToString(apiUrl)
        scriptUrl = UTF8ToString(scriptUrl)
        state = UTF8ToString(state)
        
        if (!window.unityInstance) {
          console.error("Please set `window.unityInstance` to your Unity instance before using the Bkpk SDK")
          return
        }
        const script = document.createElement('script')
        script.src = scriptUrl
        document.head.appendChild(script)
        
        script.onload = async () => {
          const bkpk = new Bkpk(clientId, { url, apiUrl })
          try {
            const result = await bkpk.authorize(responseType, {
              state,
              scopes: ['avatars:read']
            })
            if (responseType === "token") {
              window.unityInstance.SendMessage("Bkpk.WebInterface", "OnAccessToken", result.token)
            } else if (responseType === "code") {
              window.unityInstance.SendMessage("Bkpk.WebInterface", "OnAuthorizationCode", result.code)
            }
          } catch (error) {
            window.unityInstance.SendMessage("Bkpk.WebInterface", "OnUserRejected")
          }
        }
    }
});
