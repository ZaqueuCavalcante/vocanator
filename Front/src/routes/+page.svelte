<script lang="ts">
    import { emojis } from "./emojis"

    type State = 'idle' | 'playing' | 'paused' | 'won' | 'lost'

    let state: State = 'idle'
    let size: number = 20
    let grid = createGrid(size)
    var maxMatches = grid.length / 2

    let selectedCards: number[] = []
    let matches: string[] = []

    let timerId: number | null = null
    let time: number = 60

    $: selectedCards.length === 2 && matchSelectedCards()
    $: matches.length === maxMatches && gameWon()
    $: time === 0 && gameLost()

    $: state === 'playing' && (!timerId && startGameTimer())

    function createGrid(size: number): string[] {
        let cards = new Set<string>()

        while (cards.size < size / 2) {
            const randomIndex = Math.floor(Math.random() * emojis.length)
            cards.add(emojis[randomIndex])
        }

        return shuffle([...cards, ...cards])
    }

    function shuffle<T>(array: T[]) {
        return array.sort(() => Math.random() - 0.5)
    }

    function selectCard(index: number) {
        selectedCards = selectedCards.concat(index)
    }

    function matchSelectedCards() {
        const [firstIndex, secondIndex] = selectedCards

        if (grid[firstIndex] === grid[secondIndex]) {
            matches = matches.concat(grid[firstIndex])
        }

        setTimeout(() => selectedCards = [], 300)
    }

    function startGameTimer() {
        function countDown() {
            state !== 'paused' && (time -= 1)
        }

        timerId = setInterval(countDown, 1000)
    }

    function gameLost() {
        state = 'lost'
        resetGame()
    }

    function gameWon() {
        state = 'won'
        resetGame()
    }

    function pauseGame(e: KeyboardEvent) {
        if (e.key === 'Escape') {
            switch (state) {
                case 'playing':
                    state = 'paused'
                    break
                case 'paused':
                    state = 'playing'
                    break
            }
        }
    }

    function resetGame() {
        timerId && clearInterval(timerId)
        timerId = null
        time = 60
        grid = createGrid(size)
        selectedCards = []
        matches = []
    }
</script>

<svelte:window on:keydown={pauseGame}></svelte:window>

{#if state === 'idle'}
    <h1>Vocanator</h1>
    <button on:click={() => { state = 'playing' }}>Play</button>
{/if}

{#if state === 'playing'}
    <h2>Vocanator</h2>

    <h1 class="timer" class:pulse={time <= 10}>
        {time}
    </h1>

    <div class="matches">
        {#each matches as match}
            <div>{match}</div>
        {/each}
    </div>

    <div class="cards">
        {#each grid as card, index}
            {@const isSelected = selectedCards.includes(index)}
            {@const isMatched = matches.includes(card)}
            {@const isSelectedOrMatched = selectedCards.includes(index) || matches.includes(card)}

            <button
                class="card"
                class:selected={isSelected}
                disabled={isSelectedOrMatched}
                class:flip={isSelectedOrMatched}
                on:click={() => selectCard(index)}
            >
                <div class="back" class:isMatched>{card}</div>
            </button>
        {/each}
    </div>
{/if}

{#if state === 'paused'}
    <h1>Game paused</h1>
{/if}

{#if state === 'lost'}
    <h2>Vocanator</h2>
    <h1>You lost! 💩</h1>
    <button on:click={() => { state = 'playing' }}>Play again</button>
{/if}

{#if state === 'won'}
    <h2>Vocanator</h2>
    <h1>You win! 🎉</h1>
    <button on:click={() => { state = 'playing' }}>Play again</button>
{/if}

<style>
    .timer {
        transition: color 0.3s ease;
    }
    .pulse {
        color: var(--pulse);
        animation: pulse 1s infinite ease;
    }
    @keyframes pulse {
        to {
            scale: 1.4;
        }
    }

    .cards {
        display: grid;
        grid-template-columns: repeat(4, 1fr);
        gap: 0.4rem;
    }

    .matches {
        display: flex;
        gap: 0.5rem;
        margin-block: 0.5rem;
        font-size: 1.2rem;
    }

    .card {
        height: 5.1rem;
        width: 5.1rem;
        font-size: 2.8rem;
        background-color: var(--bg-2);

        transition: rotate 0.3s ease-out;
        transform-style: preserve-3d;

        &.flip {
            rotate: y 180deg;
            pointer-events: none;
        }

        & .back {
            position: absolute;
            inset: 0;
            display: grid;
            place-content: center;
            backface-visibility: hidden;
            rotate: y 180deg;
        }

        &.selected {
            border: 4px solid var(--border);
        }

        & .isMatched {
            transition: opacity 0.3s ease-out;
            opacity: 0.4;
        }
    }
</style>
