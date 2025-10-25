<script lang="ts">
  import { onMount } from 'svelte';
  import { PUBLIC_API_URL } from '$env/static/public';

  type HankingItem = {
    area: string;
    score: number;
  };

  type TestItem = {
    name: string;
    email: string;
    hanking: HankingItem[];
  };

  // estado
  let tests: TestItem[] = [];
  let total = 0;
  let isLoadingData = true;
  let error: string | null = null;

  async function loadQuestions() {
    isLoadingData = true;
    error = null;
    try {
      const res = await fetch(`${PUBLIC_API_URL}/vocational-tests`);
      if (!res.ok) {
        throw new Error(`${res.status} ${res.statusText}`);
      }
      const body = await res.json();
      // Pode ser { total, tests: [...] } ou já um array — tratamos ambos
      if (Array.isArray(body)) {
        tests = body as TestItem[];
        total = tests.length;
      } else {
        tests = (body.tests ?? body) as TestItem[];
        total = body.total ?? tests.length;
      }
    } catch (err) {
      console.error('Erro ao carregar testes vocacionais:', err);
      error = (err as Error).message || 'Erro desconhecido';
      tests = [];
      total = 0;
    } finally {
      isLoadingData = false;
    }
  }

  onMount(loadQuestions);
</script>

<svelte:head>
  <meta name="description" content="Listagem de testes vocacionais" />
</svelte:head>

<main class="page">
  <header class="header">
    <h1>{total} Testes Vocacionais</h1>
    <div class="meta">
      <button class="btn-refresh" on:click={loadQuestions} aria-label="Recarregar lista" disabled={isLoadingData}>
        ⟳ Atualizar
      </button>
    </div>
  </header>

  {#if error}
    <div class="alert">Erro ao carregar: {error}</div>
  {/if}

  {#if !isLoadingData && tests.length === 0}
    <div class="empty">Nenhum teste encontrado.</div>
  {/if}

  <section class="cards">
    {#each tests as test (test.email + test.name)}
      <article class="card" role="article" aria-labelledby={"card-" + test.email}>
        <h2 id={"card-" + test.email} class="card-title">{test.name}</h2>

        <div class="hanking">
          <ol>
            {#each test.hanking ?? [] as item, idx}
              <li class="hanking-item">
                <span class="rank">{idx + 1}.</span>
                <span class="area">{item.area}</span>
                <span class="score">{item.score}</span>
              </li>
            {/each}
            {#if !(test.hanking && test.hanking.length)}
              <li class="no-ranking">Sem ranking disponível</li>
            {/if}
          </ol>
        </div>
      </article>
    {/each}
  </section>
</main>

<style>
  :global(body) {
    font-family: system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial;
    margin: 0;
    background: #fafafa;
    color: #111;
  }

  .page {
    max-width: 1100px;
    margin: 32px auto;
    padding: 0 16px;
  }

  .header {
    display: flex;
    gap: 12px;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 18px;
  }

  .header h1 {
    margin: 0;
    font-size: 1.5rem;
  }

  .meta {
    display: flex;
    gap: 12px;
    align-items: center;
  }

  .btn-refresh {
    background: transparent;
    border: 1px solid #ddd;
    padding: 6px 10px;
    border-radius: 8px;
    cursor: pointer;
    font-size: 0.9rem;
  }

  .btn-refresh[disabled] {
    opacity: 0.6;
    cursor: not-allowed;
  }

  .alert {
    background: #ffe6e6;
    color: #8a1f1f;
    border: 1px solid #f2c0c0;
    padding: 10px 12px;
    border-radius: 8px;
    margin-bottom: 16px;
  }

  .empty {
    color: #666;
    padding: 18px;
    text-align: center;
  }

  /* Grid responsivo: no desktop vários cards lado a lado, no mobile empilhados */
  .cards {
    display: grid;
    gap: 16px;
    grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
    align-items: start;
  }

  .card {
    background: #fff;
    border: 1px solid #eee;
    border-radius: 12px;
    padding: 16px;
    box-shadow: 0 1px 4px rgba(16, 24, 40, 0.04);
    display: flex;
    flex-direction: column;
    gap: 12px;
  }

  .card-title {
    margin: 0;
    font-size: 1.05rem;
    line-height: 1.25;
    color: #0f172a;
  }

  .card-email {
    margin: 0;
    font-size: 0.9rem;
    color: #475569;
    word-break: break-all;
  }

  .hanking h3 {
    margin: 0 0 8px 0;
    font-size: 0.95rem;
  }

  .hanking ol {
    margin: 0;
    padding: 0;
    list-style: none;
    display: flex;
    flex-direction: column;
    gap: 8px;
  }

  .hanking-item {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 0.9rem;
    color: #0f172a;
  }

  .rank {
    min-width: 28px;
    text-align: center;
    font-weight: 600;
    color: #0b63ff;
  }

  .area {
    flex: 1;
	font-size: 15px;
	font-weight: 400;
  }

  .score {
    font-weight: 700;
    min-width: 48px;
    text-align: right;
  }

  .no-ranking {
    color: #667085;
    font-size: 0.9rem;
  }

  /* Mobile tweaks (opcional) — aumenta espaçamento vertical quando estreito */
  @media (max-width: 520px) {
    .page {
      margin: 20px 12px;
    }

    .card {
      padding: 14px;
    }
  }
</style>
