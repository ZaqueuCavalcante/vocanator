<script lang="ts">
  import { PUBLIC_API_URL } from "$env/static/public";
  import { onMount } from "svelte";

  let areas: { name: string }[] = [];

  async function carregar() {
    const res = await fetch(`${PUBLIC_API_URL}/knowledge-areas`);
    areas = await res.json();
  }

  onMount(carregar);
</script>

<h3>Áreas do Conhecimento</h3>

<ul>
    {#each areas as area}
        <li>{area.name}</li>
    {/each}
</ul>
