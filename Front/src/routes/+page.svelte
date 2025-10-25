<script lang="ts">
  import { PUBLIC_API_URL } from "$env/static/public";
  import { onMount } from "svelte";

  let name = $state('Zezinho');
  let email = $state('zezinho@example.com');

  let areas = $state<{ name: string }[]>([]);
  let questions = $state<{ text: string }[]>([]);

  async function loadData() {
    const res = await fetch(`${PUBLIC_API_URL}/knowledge-areas`);
    areas = await res.json();

    const resQuestions = await fetch(`${PUBLIC_API_URL}/questions`);
    questions = await resQuestions.json();
  }

  onMount(loadData);
</script>

<div style="margin: 20px;">
    <p>Bem-vindo ao Vocanator!</p>
    <input value={name} />
    <input value={email} />
</div>

<div style="margin: 20px;">
    <h3>Perguntas</h3>

    <ul>
        {#each questions as question}
            <li>{question.text}</li>
        {/each}
    </ul>
</div>

<div style="margin: 20px;">
    <h3>Áreas do Conhecimento</h3>

    <ul>
        {#each areas as area}
            <li>{area.name}</li>
        {/each}
    </ul>
</div>

<div style="margin: 20px;">
    FIM
</div>
