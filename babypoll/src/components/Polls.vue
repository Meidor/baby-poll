<template>
  <div class="polls">
    <table v-if="loaded">
      <thead>
        <tr>
          <th>name</th>
          <th>participants</th>
          <th>actions</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="poll in polls"
          v-bind:key="poll.pollId"
          v-on:click="open(poll.pollId)"
        >
          <td>{{ poll.name }}</td>
          <td>{{ poll.entries.length }}</td>
          <td class="actions">
            <ul>
              <li>👁</li>
            </ul>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Prop } from "vue-property-decorator";
import { IPoll } from "../models/poll";
@Component({
  components: {}
})
export default class Polls extends Vue {
  polls: IPoll[] = [];
  $auth: any;
  loaded: boolean = false;

  async mounted(): Promise<void> {
    this.polls = (await (
      await fetch(`${process.env.VUE_APP_API_URL}/api/poll`, {
        method: "GET"
      })
    ).json()) as IPoll[];
    let host = window.location.hostname;
    let poll = this.polls.find(x => x.url.replace(/^https?:\/\//, "") === host);
    if (poll) {
      this.open(poll.pollId);
    } else {
      this.loaded = true;
    }
  }

  open(id: string) {
    this.$router.push({
      name: "poll",
      params: { id: id }
    });
  }
}
</script>

<style scoped>
.polls {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 4rem;
}

tbody > tr:hover {
  cursor: pointer;
}
</style>
