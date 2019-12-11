import Vue from "vue";
import App from "./App.vue";
import router from "./router";
// import the Auth0 configuration
import { domain, clientId } from "../auth_config.json";

// import the plugin here
import { Auth0Plugin } from "./auth";

// install the authentication plugin here
Vue.use(Auth0Plugin, {
  domain,
  clientId,
  onRedirectCallback: (appState: any) => {
    router.push(
      appState && appState.targetUrl
        ? appState.targetUrl
        : window.location.pathname
    );
  }
});

Vue.config.productionTip = false;

new Vue({
  router,
  render: h => h(App)
}).$mount("#app");
