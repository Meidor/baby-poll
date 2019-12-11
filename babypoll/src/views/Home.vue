<template>
  <section class="home">
    <main>
      <h1>👶 Baby Polls 🍼</h1>
      <Polls></Polls>
    </main>
  </section>
</template>
<style scoped>
.auth > button {
  height: 48px;
  padding: 16px;
  text-transform: lowercase;
}

.home {
  display: flex;
  flex-direction: column;
  height: 100%;
  width: 100%;
}

main {
  flex-grow: 1;
}
</style>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Polls from "@/components/Polls.vue";

@Component({
  components: { Polls }
})
export default class Home extends Vue {
  $auth: any;
  login() {
    this.$auth.loginWithRedirect();
  }

  logout() {
    this.$auth.logout({
      returnTo: window.location.origin
    });
  }
}
</script>

<style scoped>
</style>
