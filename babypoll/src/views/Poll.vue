<template>
  <div class="root" v-if="poll">
    <h1>👶 {{ poll.name }} 🍼</h1>
    <h2>Due Date: {{ dueDateString }}</h2>
    <div class="poll">
      <table>
        <thead>
          <tr>
            <th>Participant</th>
            <th>Guess</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="entry in poll.entries" v-bind:key="entry.entryId">
            <td>{{ entry.participant }}</td>
            <td>{{ formatDate(entry.guess) }}</td>
          </tr>
          <tr>
            <td>
              <input
                type="text"
                name="participant"
                placeholder="your name"
                v-model="participant"
                required
              />
            </td>
            <td>
              <datepicker
                name="guess"
                placeholder="birthday"
                v-model="guess"
                :monday-first="true"
                :typeable="true"
                :use-utc="true"
                :disabled-dates="disabledDates"
                :highlighted="highlightedDates"
                :open-date="dueDate"
                :format="format"
                required
              ></datepicker>
            </td>
            <td class="actions">
              <button
                id="entry-submit"
                v-on:click="submit"
                :disabled="!canSubmit"
              >
                ➕
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
<script lang="ts">
import { Component, Vue } from "vue-property-decorator";
import { IPoll } from "../models/poll";
import Datepicker from "vuejs-datepicker";
import { IEntry } from "../models/entry";

@Component({
  components: { Datepicker }
})
export default class Home extends Vue {
  $auth: any;
  poll: IPoll | null = null;
  participant: string | null = null;
  guess: Date | null = null;
  submitting: boolean = false;
  format: string = "yyyy-MM-dd";

  get isValid() {
    return (
      this.guess !== null &&
      this.poll !== null &&
      this.participant !== null &&
      !this.isNullOrWhitespace(this.participant) &&
      this.poll.entries.filter(
        e =>
          e.participant === this.participant ||
          this.sameDate(this.guess, new Date(e.guess))
      ).length === 0
    );
  }

  get canSubmit() {
    return this.isValid && !this.submitting;
  }

  sameDate(a: Date | null, b: Date | null): boolean {
    return (
      a !== null &&
      b !== null &&
      a.getFullYear() === b.getFullYear() &&
      a.getMonth() === b.getMonth() &&
      a.getDate() === b.getDate()
    );
  }

  isNullOrWhitespace(input: string) {
    if (typeof input === "undefined" || input == null) return true;
    return input.replace(/\s/g, "").length < 1;
  }

  get dueDate(): Date | null {
    if (this.poll) {
      return new Date(this.poll.dueDate);
    }
    return null;
  }

  get dueDateString(): string {
    if (this.poll) {
      var date = new Date(this.poll.dueDate);
      let year = date.getFullYear();
      let month = (date.getMonth() + 1).toString().padStart(2, "0");
      let day = date
        .getDate()
        .toString()
        .padStart(2, "0");
      return `${year}-${month}-${day}`;
    }
    return "";
  }

  get highlightedDates():
    | { dates: Date[]; includeDisabled: boolean }
    | undefined {
    if (this.poll == null) return undefined;
    return {
      dates: [new Date(this.poll.dueDate)],
      includeDisabled: true
    };
  }

  get disabledDates(): { dates: Date[] } | undefined {
    if (this.poll == null) return undefined;
    return {
      dates: this.poll.entries.map(e => new Date(e.guess))
    };
  }

  formatDate(input: string) {
    let date = new Date(input);
    let year = date.getUTCFullYear();
    let month = (date.getUTCMonth() + 1).toString().padStart(2, "0");
    let day = date
      .getUTCDate()
      .toString()
      .padStart(2, "0");
    return `${year}-${month}-${day}`;
  }

  async submit() {
    this.submitting = true;
    try {
      if (!this.participant || !this.guess || !this.poll) {
        return;
      }
      let entry: IEntry = {
        pollId: this.poll.pollId,
        participant: this.participant,
        guess: this.guess.toISOString()
      };
      await fetch(
        `${process.env.VUE_APP_API_URL}/api/poll/${this.$route.params.id}/entry`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify(entry)
        }
      );
    } finally {
      this.participant = null;
      this.guess = null;
      this.submitting = false;
    }
    this.updatePoll();
  }

  async updatePoll() {
    if (this.$route.params.id) {
      this.poll = (await (
        await fetch(
          `${process.env.VUE_APP_API_URL}/api/poll/${this.$route.params.id}`
        )
      ).json()) as IPoll;
      if (this.poll) {
        document.title = `Poll ${this.poll.name}`;
      }
    }
  }

  async mounted() {
    await this.updatePoll();
  }
}
</script>

<style scoped>
.actions button {
  height: 42px;
  width: 42px;
  transform: none;
  transition: none;
}
.poll {
  display: flex;
  flex-direction: column;
  align-items: center;
  z-index: 10;
}

#entry-submit:disabled {
  color: gray;
  cursor: not-allowed;
}
</style>
