<template>
  <div id="app">
    <div id="theme-switcher">
      <button v-on:click="toggleTheme()"></button>
    </div>
    <router-view class="router" />
    <Baby id="#baby"></Baby>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import Baby from "./components/Baby.vue";
@Component({
  components: { Baby }
})
export default class App extends Vue {
  toggleTheme(): void {
    var style = getComputedStyle(document.documentElement);
    var currBackground = style.getPropertyValue("--background-color");
    localStorage.setItem("currBackground", currBackground);
    this.setBackground(currBackground);
  }

  setBackground(currBackground: string) {
    var style = getComputedStyle(document.documentElement);
    var babyBlue = style.getPropertyValue("--baby-blue");
    var babyPink = style.getPropertyValue("--baby-pink");
    if (currBackground === babyBlue) {
      document.documentElement.style.setProperty(
        "--background-color",
        babyPink
      );
      document.documentElement.style.setProperty("--accent-color", babyBlue);
    } else {
      document.documentElement.style.setProperty(
        "--background-color",
        babyBlue
      );
      document.documentElement.style.setProperty("--accent-color", babyPink);
    }
  }

  mounted() {
    var currBackground = localStorage.getItem("currBackground");
    if (currBackground) {
      this.setBackground(currBackground);
    } else {
      var style = getComputedStyle(document.documentElement);
      var babyBlue = style.getPropertyValue("--baby-blue");
      var babyPink = style.getPropertyValue("--baby-pink");
      var rand = Math.random();
      if (rand > 0.5) {
        this.setBackground(babyPink);
      } else {
        this.setBackground(babyBlue);
      }
    }
  }
}
</script>
<style>
:root {
  --baby-blue: #89cff0;
  --baby-pink: #f4c2c2;
  --background-color: white;
  --accent-color: white;
  --text-color: black;
  --inverse-text-color: white;
  --color-item: rgba(255, 255, 255, 0.9);
}

* {
  transition: background-color 300ms ease-in-out;
}

body {
  padding: 0;
  margin: 0;
  background: var(--background-color);
  color: var(--text-color);
}

#baby {
  z-index: 1;
}

.router {
  flex-grow: 1;
  z-index: 2;
}

#theme-switcher {
  position: fixed;
  bottom: 8px;
  right: 8px;
  z-index: 3;
}
#theme-switcher > button {
  width: 42px;
  height: 42px;
  border-radius: 50%;
  border: none;
  background-color: var(--accent-color);
}

#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: var(--text-color);
}

#nav a.router-link-exact-active {
  color: var(--accent-color);
}

table {
  border-spacing: 0;
  min-width: 75vw;
  margin-top: 4rem;
  background: var(--color-item);
  padding: 64px;
  border-radius: 8px;
  flex-grow: 1;
}

thead tr {
  height: 42px;
}

tbody tr {
  height: 52px;
}

th,
td {
  text-align: start;
}

th {
  padding: 0 16px;
  border-top: 2px solid var(--text-color);
  border-bottom: 2px solid var(--text-color);
}

td {
  padding: 0 16px;
  border-bottom: 1px solid var(--text-color);
}

.actions {
  font-size: 2rem;
}

.actions button {
  background: none;
  border: none;
  font-size: 1.2rem;
}

.actions button:hover {
  cursor: pointer;
}

.actions > ul {
  list-style: none;
  margin: 0;
  padding: 0;
}

.actions li {
  display: inline;
  padding-right: 1rem;
}

.actions li:hover {
  cursor: pointer;
  color: var(--color-accent);
}

input {
  border: none;
  border-bottom: 1px solid var(--text-color);
  font-size: 1.2rem;
  line-height: 2rem;
  background: var(--color-item);
}

input[type="text"] {
  background: transparent;
}
</style>
