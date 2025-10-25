<script lang="ts">
	import { PUBLIC_API_URL } from '$env/static/public';
	import { onMount } from 'svelte';

	// --- ESTADO GLOBAL DA PÁGINA ---
	let pageState = $state<'home' | 'quiz' | 'perfil'>('home');
	let modalVisible = $state(false);
	let showCelebration = $state(false);

	// --- DADOS DO USUÁRIO ---
	let user = $state({ name: '', email: '' });
	let userNameInput = $state('');
	let userEmailInput = $state('');

	// --- DADOS DA API ---
	// 'areas' não será mais carregado da API, agora é estático
	let questions = $state<any[]>([]); // Será preenchido por /questions

	// --- OPÇÕES ESTÁTICAS DO QUIZ ---
	const quizOptions = [
		{ texto: 'Sim', valor: true },
		{ texto: 'Não', valor: false }
	];

	// --- ESTADO DO QUIZ ---
	let currentQuestionIndex = $state(0);
	let answers = $state<Record<number, boolean>>({});
	let isLoadingData = $state(true); // Estado de loading inicial para perguntas
	let isLoadingProfile = $state(false); // Estado de loading para análise do perfil
	let rankingData = $state<any[]>([]);

	// --- DADOS COMPUTADOS (DERIVED) ---
	let currentQuestion = $derived(questions[currentQuestionIndex]);
	let currentAnswer = $derived(answers[currentQuestionIndex]);
	let quizProgress = $derived(
		questions.length > 0
			? (currentQuestionIndex / questions.length) * 100 // usa apenas o índice atual
			: 0
	);

	let showPlusOne = $state(false); // controla o "+1"
	let plusOneStyle = $state({ top: '0px', left: '0px' }); // posição dinâmica

	function triggerPlusOne(event: MouseEvent) {
		plusOneStyle = {
			top: `${event.clientY - 20}px`,
			left: `${event.clientX}px`
		};
		showPlusOne = true;

		setTimeout(() => {
			showPlusOne = false;
		}, 800); // duração da animação
	}

	// --- DADOS ESTÁTICOS PARA INTELIGÊNCIAS (HOME) - AGORA OFFLINE ---
	const staticInteligencias = [
		{
			name: 'Lógico-matemática',
			icon: 'fas fa-calculator',
			description: 'Habilidade com números, lógica, resolução de problemas e raciocínio.'
		},
		{
			name: 'Linguística',
			icon: 'fas fa-book-open',
			description: 'Habilidade com palavras, comunicação verbal e escrita. Facilidade em se expressar.'
		},
		{
			name: 'Espacial',
			icon: 'fas fa-cube',
			description: 'Capacidade de visualizar e manipular objetos mentalmente. Importante para artes e design.'
		},
		{
			name: 'Musical',
			icon: 'fas fa-music',
			description: 'Sensibilidade para ritmo, melodia e sons. Facilidade em compor e tocar.'
		},
		{
			name: 'Corporal-cinestésica',
			icon: 'fas fa-running',
			description: 'Habilidade em usar o corpo, coordenação motora e expressão física.'
		},
		{
			name: 'Interpessoal',
			icon: 'fas fa-users',
			description: 'Entendimento sobre os outros, empatia, capacidade de se relacionar e liderar.'
		},
		{
			name: 'Intrapessoal',
			icon: 'fas fa-brain',
			description: 'Autoconhecimento, capacidade de reflexão e entendimento das próprias emoções.'
		},
		{
			name: 'Naturalista',
			icon: 'fas fa-leaf',
			description: 'Sensibilidade para compreender e organizar padrões da natureza e meio ambiente.'
		}
	];


	// --- MAPA PARA ÁREAS DO RANKING (PERFIL) ---
	const rankingAreaDetailsMap: Record<string, { icon: string; description: string }> = {
		"Engenharia e Tecnologia": { icon: 'fas fa-cogs', description: 'Criação e aplicação de soluções tecnológicas e sistemas complexos.' },
		"Ciências Exatas": { icon: 'fas fa-flask', description: 'Estudo de matemática, física, química e suas aplicações práticas.' },
		"Ciências Humanas": { icon: 'fas fa-book-reader', description: 'Análise da sociedade, cultura, história e comportamento humano.' },
		"Saúde": { icon: 'fas fa-heartbeat', description: 'Cuidado com o bem-estar físico e mental, pesquisa e tratamento.' },
		"Artes e Design": { icon: 'fas fa-paint-brush', description: 'Expressão criativa através de formas visuais, sonoras ou performáticas.' },
		"Comunicação": { icon: 'fas fa-bullhorn', description: 'Transmissão de informações e ideias através de diferentes mídias.' },
		"Empreendedorismo": { icon: 'fas fa-lightbulb', description: 'Identificação de oportunidades, inovação e liderança de negócios.' },
		"Educação": { icon: 'fas fa-chalkboard-teacher', description: 'Transmissão de conhecimento e desenvolvimento de habilidades.' },
		// Adicione mais áreas conforme necessário
	};
	const defaultRankingDetails = { icon: 'fas fa-star', description: 'Área promissora baseada em seu perfil.' };

	function shuffleArray<T>(array: T[]): T[] {
        const newArray = [...array]; // copia para não alterar o original
        for (let i = newArray.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1)); // índice aleatório
            [newArray[i], newArray[j]] = [newArray[j], newArray[i]]; // troca os elementos
        }
        return newArray;
	}

	// --- CARREGAMENTO DE DADOS (APENAS PERGUNTAS) ---
	async function loadQuestions() {
		isLoadingData = true;
		try {
			const resQuestions = await fetch(`${PUBLIC_API_URL}/questions`);
			if (resQuestions.ok) {
				let allQuestions = await resQuestions.json();
				questions = shuffleArray(allQuestions)
				console.log('Perguntas carregadas (máx 15):', questions);
			} else {
				console.error('Falha ao carregar perguntas:', resQuestions.statusText);
				questions = [];
			}
		} catch (error) {
			console.error('Falha ao carregar perguntas:', error);
			questions = [];
		} finally {
			isLoadingData = false;
		}
	}

	onMount(loadQuestions); // Chama apenas o carregamento de perguntas

	// --- NAVEGAÇÃO E AÇÕES ---
	async function handleRegistration() {
		user.name = userNameInput;
		user.email = userEmailInput;
		modalVisible = false;
		startQuiz();
	}

	function startQuiz() {
		if (questions.length === 0 && !isLoadingData) {
			alert("As perguntas não puderam ser carregadas. Verifique sua conexão ou tente mais tarde.");
			return;
		}
        if (isLoadingData) {
             alert("Aguarde o carregamento dos dados antes de iniciar.");
             return;
        }
		pageState = 'quiz';
		currentQuestionIndex = 0;
		answers = {};
		rankingData = []; // Limpa dados anteriores do ranking
		showCelebration = false; // Garante que a celebração não está ativa
	}

	function selectAnswer(value: boolean) {
		answers[currentQuestionIndex] = value;
	}

	function handleQuizNext() {
		if (answers[currentQuestionIndex] === undefined) {
			return; // Não permite avançar sem resposta
		}

		triggerPlusOne(event);

		if (currentQuestionIndex < questions.length - 1) {
			currentQuestionIndex++;
		} else {
			submitQuiz();
		}
	}

	function handleQuizPrevious() {
		if (currentQuestionIndex > 0) {
			currentQuestionIndex--;
		}
	}

	async function submitQuiz() {
		isLoadingProfile = true;
		pageState = 'perfil';
		showCelebration = false;

		const payload = {
			Name: user.name,
			Email: user.email,
			Responses: questions.map((question, index) => ({
				QuestionId: question.id,
				Yes: answers[index] ?? false
			}))
		};

		console.log('Enviando payload (POST):', payload);

		try {
			// --- PASSO 1: FAZ O POST (simulado) ---
			// TODO: Substituir pela chamada POST real
            const res = await fetch( `${PUBLIC_API_URL}/vocational-tests`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload),
                });
            
            const result = await res.json();

            let hanking: any = {};

            try {
                const responseHanking = await fetch(`${PUBLIC_API_URL}/vocational-tests/${result.id}`);
                if (responseHanking.ok) {
                    hanking = await responseHanking.json();
                    console.log('Hanking:', hanking);
                } else {
                    console.error('Falha ao carregar perguntas:', responseHanking.statusText);
                    hanking = {};
                }
            } catch (error) {
                console.error('Falha ao carregar perguntas:', error);
                hanking = {};
            } finally {
                isLoadingData = false;
            }

			await new Promise(resolve => setTimeout(resolve, 1000));
			const testId = Date.now();
			console.log('POST simulado retornou ID:', testId);

			if (!testId) throw new Error('ID do teste não recebido');

			// --- PASSO 2: FAZ O GET (simulado) ---
			// TODO: Substituir pela chamada GET real
			const rankingResult = {
				hanking: hanking.hanking || []
			};
			console.log('GET simulado retornou:', rankingResult);

			rankingData = rankingResult.hanking || [];
			showCelebration = true; // Ativa a celebração após sucesso

		} catch (error) {
			console.error("Erro ao submeter/buscar resultados:", error);
			alert("Não foi possível carregar seu perfil. Tente novamente.");
			pageState = 'quiz';
		} finally {
			isLoadingProfile = false;
		}
	}
</script>

<style>
	/* --- Estilos Gerais --- */
	* { box-sizing: border-box; margin: 0; padding: 0; }
	body { font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; background-color: #f9f9f9; color: #333; line-height: 1.6; }

	.btn { display: inline-flex; align-items: center; gap: 8px; padding: 14px 32px; border-radius: 8px; text-decoration: none; font-weight: bold; font-size: 1.1em; cursor: pointer; border: 1px solid transparent; transition: all 0.2s ease; }
	.btn:hover { transform: translateY(-2px); }
	.btn-principal { background-color: #007bff; color: #ffffff; border: none; }
	.btn-principal[disabled] { background-color: #a0cfff; cursor: not-allowed; }
	.btn-principal i { margin-left: 8px; }
	.btn-outline { background-color: transparent; color: #555; border-color: #ccc; padding: 10px 20px; font-size: 1em; font-weight: 600; }
	.btn-outline:hover { background-color: #f0f0f0; }
	.btn-outline:disabled { color: #ccc; border-color: #eee; cursor: not-allowed; }

	.section-subtitle { font-size: 1.1em; color: #555; max-width: 700px; margin: 0 auto 40px auto; text-align: center; }
	.tag { display: inline-block; padding: 4px 12px; border-radius: 20px; font-size: 0.9em; font-weight: 500; flex-shrink: 0; }
	.tag-blue { background-color: #e6f2ff; color: #007bff; }
	.tag-gray { background-color: #f0f0f0; color: #555; border: 1px solid #ddd; }

	/* --- Modal --- */
	.modal-overlay { position: fixed; top: 0; left: 0; right: 0; bottom: 0; background-color: rgba(0, 0, 0, 0.6); display: flex; justify-content: center; align-items: center; z-index: 2000; padding: 20px; }
	.modal-content { background-color: #ffffff; padding: 30px; border-radius: 12px; width: 100%; max-width: 400px; text-align: center; position: relative; }
	.modal-content h2 { font-size: 1.8em; color: #2c3e50; margin-bottom: 10px; }
	.modal-content p { font-size: 1em; color: #555; margin-bottom: 25px; }
	.form-grupo { margin-bottom: 15px; text-align: left; }
	.form-grupo label { display: block; font-weight: 600; margin-bottom: 5px; color: #333; }
	.form-grupo input { width: 100%; padding: 12px; font-size: 1em; border: 1px solid #ccc; border-radius: 8px; }
	.form-grupo input:focus { outline: none; border-color: #007bff; box-shadow: 0 0 5px rgba(0, 123, 255, 0.3); }
	.modal-content .btn-principal { width: 100%; }

	/* --- Navbar --- */
	.navbar { display: flex; justify-content: space-between; align-items: center; padding: 15px 25px; background-color: #ffffff; border-bottom: 1px solid #e0e0e0; }
	.logo { font-size: 1.8em; font-weight: bold; color: #007bff; text-decoration: none; } /* Logo como texto */
	.perfil-link { text-decoration: none; color: #333; font-weight: 600; font-size: 1em; }
	.perfil-link i { margin-right: 8px; color: #007bff; }

	/* --- Hero Section --- */
	.hero-section { text-align: center; padding: 80px 20px; background-color: #ffffff; }
	.hero-badge { display: inline-block; padding: 8px 15px; border-radius: 20px; background-color: #e6f2ff; color: #007bff; font-size: 0.9em; font-weight: 600; margin-bottom: 25px; }
	.hero-badge i { margin-right: 6px; }
	.hero-section h1 { font-size: 2.8em; font-weight: 700; color: #222; max-width: 650px; margin: 0 auto 20px auto; line-height: 1.2; }
	.hero-section p { font-size: 1.1em; color: #555; max-width: 600px; margin: 0 auto 35px auto; }

	.plus-one {
		position: fixed;
		color: #007bff;
		font-size: 1.5em;
		font-weight: bold;
		pointer-events: none;
		animation: floatUp 0.8s ease-out forwards;
		z-index: 10000;
	}

	@keyframes floatUp {
		0% { transform: translateY(0) scale(1); opacity: 1; }
		50% { transform: translateY(-20px) scale(1.2); opacity: 1; }
		100% { transform: translateY(-60px) scale(1); opacity: 0; }
	}

	/* --- Como Funciona Section --- */
	.como-funciona-section { padding: 60px 20px; background-color: #f9f9f9; }
	.como-funciona-section h2 { font-size: 2em; color: #2c3e50; margin-bottom: 15px; text-align: center; }
	.passos-container { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 20px; max-width: 1200px; margin: 0 auto; }
	.passo-card { background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 12px; padding: 30px; text-align: left; }
	.passo-card.highlight { border-color: #007bff; box-shadow: 0 4px 15px rgba(0, 123, 255, 0.1); }
	.passo-icon { font-size: 1.5em; color: #007bff; background-color: #e6f2ff; width: 45px; height: 45px; border-radius: 50%; display: flex; align-items: center; justify-content: center; margin-bottom: 20px; }
	.passo-card h3 { font-size: 1.2em; font-weight: 600; color: #333; margin-bottom: 10px; }
	.passo-card p { font-size: 0.95em; color: #555; line-height: 1.5; }

	/* --- Inteligências Múltiplas Section (Home) --- */
	.inteligencias-section { padding: 60px 20px; background-color: #ffffff; }
	.inteligencias-section h2 { font-size: 2.2em; color: #2c3e50; margin-bottom: 15px; text-align: center; }
	.inteligencias-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(280px, 1fr)); gap: 20px; max-width: 1200px; margin: 40px auto 0 auto; }
	.inteligencia-card { background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 12px; padding: 25px; display: flex; gap: 15px; align-items: flex-start; }
	.inteligencia-card .icon { font-size: 1.8em; color: #007bff; margin-top: 5px; flex-shrink: 0; width: 30px; text-align: center; }
	.inteligencia-card div { flex-grow: 1; }
	.inteligencia-card h5 { font-size: 1.1em; color: #333; margin-bottom: 3px; font-weight: 600; }
	.inteligencia-card p { font-size: 0.9em; color: #555; line-height: 1.5; margin: 0; }

	/* --- Footer --- */
	.footer { background-color: #2c3e50; color: #bdc3c7; padding: 50px 20px 20px 20px; text-align: left; }
	.footer-container { display: flex; flex-wrap: wrap; justify-content: space-between; max-width: 1200px; margin: 0 auto; gap: 30px; }
	.footer-col { flex: 1; min-width: 200px; }
	.footer-col h6 { font-size: 1.2em; color: #ffffff; margin-bottom: 15px; font-weight: bold;} /* Restaurado estilo texto logo */
	.footer-col ul { list-style: none; }
	.footer-col ul li { margin-bottom: 8px; }
	.footer-col ul li a { color: #bdc3c7; text-decoration: none; }
	.footer-col ul li a:hover { color: #ffffff; }
	.copyright { text-align: center; margin-top: 40px; padding-top: 20px; border-top: 1px solid #4e5d6c; font-size: 0.9em; max-width: 1200px; margin-left: auto; margin-right: auto; }

	/* --- Quiz Page --- */
	.quiz-page-container { display: flex; justify-content: center; align-items: center; min-height: 100vh; padding: 20px; background-color: #f4f7f6; }
	.quiz-container { width: 100%; max-width: 550px; }
	.progress-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 8px; font-size: 0.9em; font-weight: 600; }
	#pergunta-atual { color: #333; }
	#porcentagem-completa { color: #007bff; }
	

	

.progress-bar-container {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 20px;
}

.start-icon, .end-icon {
  font-size: 1.5em;
  color: #ffcc00; /* dourado */
}

.progress-bar-track {
  flex-grow: 1;
  height: 24px; /* altura maior para parecer barra de XP */
  background-color: #ccc; /* cor do fundo da barra */
  border-radius: 12px;
  overflow: hidden;
  position: relative;
}

.progress-bar-fill {
  height: 100%;
  background: linear-gradient(90deg, #00aaff, #007bff); /* gradiente azul */
  border-radius: 12px 0 0 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: width 0.4s ease;
  position: relative;
}

.progress-percentage {
  color: #fff;
  font-weight: bold;
  font-size: 0.9em;
  position: absolute;
  left: 50%;
  transform: translateX(-50%);
}

	

	
	.quiz-card { background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 12px; padding: 30px; box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05); }
	.quiz-badge { display: inline-block; padding: 6px 12px; border-radius: 20px; background-color: #e6f2ff; color: #007bff; font-size: 0.9em; font-weight: 600; margin-bottom: 15px; }
	.quiz-card h2 { font-size: 1.6em; font-weight: 700; color: #222; margin-bottom: 25px; }
	.options-container { display: flex; flex-direction: column; gap: 12px; }
	.option-card { display: flex; align-items: center; padding: 15px; border: 2px solid #e0e0e0; border-radius: 10px; cursor: pointer; transition: border-color 0.2s ease, background-color 0.2s ease; }
	.option-card:hover { background-color: #f9f9f9; }
	.option-card.selected { border-color: #007bff; background-color: #f7fbff; }
	.option-card input[type='radio'] { display: none; }
	.radio-custom { width: 22px; height: 22px; border: 2px solid #ccc; border-radius: 50%; margin-right: 15px; display: flex; align-items: center; justify-content: center; transition: border-color 0.2s ease; flex-shrink: 0; }
	.option-card.selected .radio-custom { border-color: #007bff; }
	.radio-custom::after { content: ''; width: 12px; height: 12px; background-color: #007bff; border-radius: 50%; transform: scale(0); transition: transform 0.2s ease; }
	.option-card.selected .radio-custom::after { transform: scale(1); }
	.option-text { font-size: 1em; font-weight: 500; color: #333; }

    
    .navigation-buttons {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-top: 30px;
  border-top: 1px solid #f0f0f0;
  padding-top: 20px;
  gap: 10px; /* opcional, espaçamento entre os botões */
}

/* Quando a tela for menor que 768px (mobile) */
@media (max-width: 768px) {
  .navigation-buttons {
    flex-direction: column; /* empilha os botões */
    align-items: stretch;   /* faz cada botão ocupar toda a largura */
  }

  .navigation-buttons button {
    width: 100%; /* cada botão ocupa toda a linha */
  }
}
    
    .dica-box { background-color: #e6f2ff; border: 1px solid #b3d7ff; border-radius: 10px; padding: 15px; margin-top: 20px; text-align: center; font-size: 0.9em; color: #333; }
	.dica-box strong { color: #0056b3; }

	/* --- Perfil Page (Resultados) --- */
	.perfil-page-container { max-width: 1000px; margin: 20px auto; display: grid; gap: 20px; position: relative; } /* Removido overflow: hidden */
	.perfil-card { background-color: #ffffff; border: 1px solid #e0e0e0; border-radius: 12px; padding: 25px; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03); }
	.header-perfil { display: flex; justify-content: space-between; align-items: center; background-color: #ffffff; padding: 20px; border-radius: 12px; }
	.header-perfil-info { display: flex; align-items: center; gap: 15px; }
	.avatar { width: 50px; height: 50px; border-radius: 50%; background-color: #e6f2ff; color: #007bff; display: flex; align-items: center; justify-content: center; font-size: 1.2em; font-weight: bold; flex-shrink: 0; }
	.user-details h2 { font-size: 1.3em; font-weight: 600; color: #222; margin: 0; }
	.user-details p { font-size: 0.95em; color: #555; margin: 2px 0 0 0; line-height: 1.4; }
	.card-header { display: flex; align-items: center; gap: 10px; margin-bottom: 5px; }
	.card-header .icon { font-size: 1.1em; color: #007bff; }
	.card-header h3 { font-size: 1.1em; color: #2c3e50; margin: 0; font-weight: 600; }
	.card-subtitle { font-size: 0.9em; color: #777; margin: -5px 0 15px 0; }

	.carreiras-ranking-list { display: flex; flex-direction: column; gap: 15px; margin-top: 15px; }
	.carreira-ranked-item { display: flex; align-items: flex-start; gap: 15px; background-color: #fdfdfd; border: 1px solid #e8e8e8; border-radius: 10px; padding: 20px; }
	.rank-number { font-size: 1.6em; font-weight: 700; color: #007bff; min-width: 25px; text-align: center; margin-top: 2px; }
    .ranking-icon { font-size: 1.4em; color: #555; width: 30px; text-align: center; margin-top: 3px; }
	.carreira-info { flex-grow: 1; }
	.carreira-info h4 { font-size: 1.05em; font-weight: 600; color: #333; margin-bottom: 3px; }
	.carreira-info p { font-size: 0.9em; color: #555; margin-bottom: 0; line-height: 1.4; }
	.carreira-ranked-item .tag-blue { font-size: 0.85em; font-weight: 600; margin-left: auto; align-self: center; }

	/* --- Loading States --- */
	.loading-container { display: flex; flex-direction: column; align-items: center; justify-content: center; padding: 100px 20px; text-align: center; min-height: 300px; }
	.loading-container h2 { font-size: 1.5em; color: #333; margin-top: 20px; }
	.spinner { border: 4px solid #f3f3f3; border-top: 4px solid #007bff; border-radius: 50%; width: 40px; height: 40px; animation: spin 1s linear infinite; }
	@keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

	/* --- Celebration Animation (Aprimorada) --- */
	.celebration {
		position: fixed; /* fixo na tela */
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		pointer-events: none;
		overflow: hidden;
		z-index: 5000; /* Z-index MUITO alto */
	}
	.particle {
		position: absolute;
		width: 15px; /* MAIOR */
		height: 15px; /* MAIOR */
		border-radius: 50%;
		opacity: 0;
		/* Animação com nome, duração, timing e estado final */
		animation: burst 2s ease-out forwards; /* MAIS TEMPO */
	}
	@keyframes burst {
		0% { transform: translate(0, 0) scale(0.1); opacity: 1; }
        20% { opacity: 1; } /* Mantém opaco por mais tempo no início */
		100% { transform: translate(var(--x), var(--y)) scale(0.6); /* Termina um pouco menor */ opacity: 0; }
	}


	/* --- Responsividade --- */
	@media (max-width: 768px) {
		.hero-section h1 { font-size: 2.2em; }
		.como-funciona-section h2, .inteligencias-section h2 { font-size: 1.8em; }
		.inteligencias-grid { grid-template-columns: 1fr; }
		.passos-container { grid-template-columns: 1fr; }
		.logo { font-size: 1.5em; } /* Restaurado estilo texto logo */
		.perfil-link { font-size: 0.9em; }
	}

	@media (max-width: 600px) {
		.header-perfil { flex-direction: column; align-items: flex-start; gap: 15px; }
		.carreira-ranked-item { align-items: flex-start; position: relative; padding-bottom: 40px; }
        .rank-number { align-self: flex-start; }
        .ranking-icon { align-self: flex-start; margin-top: 5px; }
		.carreira-info { width: 100%; }
        .carreira-ranked-item .tag-blue { position: absolute; bottom: 15px; right: 15px; margin-left: 0; align-self: auto; }
	}
</style>
{#if pageState === 'home'}
	{#if modalVisible}
		<div class="modal-overlay" id="modal-cadastro">
			<div class="modal-content">
				<h2>Bem-vindo!</h2>
				<p>Cadastre-se para descobrir quais áreas do conhecimento combinam com você.</p>
				<form on:submit|preventDefault={handleRegistration}>
					<div class="form-grupo">
						<label for="nome">Seu Nome</label>
						<input
							type="text"
							id="nome"
							placeholder="João da Silva"
							required
							bind:value={userNameInput}
						/>
					</div>
					<div class="form-grupo">
						<label for="email">Seu E-mail</label>
						<input
							type="email"
							id="email"
							placeholder="joao@email.com"
							required
							bind:value={userEmailInput}
						/>
					</div>
					<button type="submit"
					                            style="
                                display: flex;
                                align-items: center;
                                justify-content: center;
                                gap: 8px;
                                text-align: center;
                            "
					
					class="btn btn-principal">Cadastrar e Iniciar</button>
				</form>
			</div>
		</div>
	{/if}
	<div id="pagina-principal">
		<header class="navbar">
			<a href="#" class="logo">Vocanator</a> <a href="#" class="perfil-link" id="perfil-link">
				<i class="fas fa-user-circle"></i>
				<span>{user.name ? `Olá, ${user.name.split(' ')[0]}` : 'Seu Perfil'}</span>
			</a>
		</header>

		<main>
			<section class="hero-section">
				<h1>Descubra quais áreas do conhecimento combinam com você</h1>
				<p>
					Use inteligência artificial e testes gamificados para descobrir suas habilidades, interesses e
					o caminho profissional perfeito para você.
				</p>
				<button class="btn btn-principal" on:click={() => (modalVisible = true)}>
					Comece agora
					<i class="fas fa-arrow-right"></i>
				</button>
			</section>

			<section class="como-funciona-section">
				<h2>Como funciona</h2>
				<p class="section-subtitle">Dois passos simples para descobrir suas áreas de interesse</p>
				<div class="passos-container">
					<div class="passo-card">
						<div class="passo-icon"><i class="fas fa-check-circle"></i></div>
						<h3>Responda perguntas rápidas</h3>
						<p>
							Complete nosso teste quiz interativo com perguntas sobre seus interesses,
							habilidades e personalidade.
						</p>
					</div>
					<div class="passo-card">
						<div class="passo-icon"><i class="fas fa-bullseye"></i></div>
						<h3>Descubra quais áreas do conhecimento combinam com você</h3>
						<p>
							Nossa IA analisa suas respostas e identifica seu perfil único, sugerindo as áreas do
							conhecimento que mais se alinham com você.
						</p>
					</div>
				</div>
			</section>

			<section class="inteligencias-section">
				<h2>Teoria das Inteligências Múltiplas</h2>
				<p class="section-subtitle">
					Desenvolvida por Howard Gardner, a teoria das Inteligências Múltiplas revolucionou nossa
					compreensão sobre as capacidades humanas. Gardner propôs que não existe apenas um tipo de
					inteligência, mas sim múltiplas formas de ser inteligente. Nosso quiz é
					baseado nesta teoria científica para identificar suas habilidades únicas.
				</p>

				<div class="inteligencias-grid">
					{#each staticInteligencias as area}
						<div class="inteligencia-card">
							<i class="{area.icon} icon"></i>
							<div>
								<h5>{area.name}</h5>
								<p>{area.description}</p>
							</div>
						</div>
					{/each}
				</div>
			</section>
		</main>

		<footer class="footer">
			<div class="footer-container">
				<div class="footer-col">
					<h6>Vocanator</h6> <p style="font-size: 0.9em;">Descubra sua verdadeira vocação.</p>
				</div>
				<div class="footer-col">
					<h6>Plataforma</h6>
					<ul>
						<li><a href="#" on:click={() => (modalVisible = true)}>Fazer teste</a></li>
						<li><a href="#">Meu Perfil</a></li>
					</ul>
				</div>
				<div class="footer-col">
					<h6>Legal</h6>
					<ul>
						<li><a href="#">Termos de Uso</a></li>
						<li><a href="#">Política de Privacidade</a></li>
					</ul>
				</div>
			</div>
			<div class="copyright">© 2025 Vocanator. Todos os direitos reservados.</div>
		</footer>
		</div>
	{:else if pageState === 'quiz'}
	<div class="quiz-page-container">
		<div class="quiz-container" id="quiz-container">


{#if showPlusOne}
    <div class="plus-one" style="top: {plusOneStyle.top}; left: {plusOneStyle.left};">+1</div>
{/if}

			{#if questions.length === 0 && isLoadingData}
				<div class="loading-container">
					<div class="spinner"></div>
					<h2>Carregando perguntas...</h2>
				</div>
			{:else if questions.length > 0 && currentQuestion}
				<div class="progress-bar-container">
					<div class="progress-bar-track">
						<div class="progress-bar-fill" style="width: {quizProgress}%;">
						<span class="progress-percentage" style="opacity: {quizProgress < 5 ? 0 : 1}">
							{Math.round(quizProgress)}%
						</span>
						</div>
					</div>

					<i class="fas fa-trophy end-icon"></i> <!-- ícone final/premio -->
				</div>

				<div class="quiz-card">
					<h2 id="question-text">{currentQuestion.text || 'Carregando...'}</h2>

					<div class="options-container" id="options-container">
						{#each quizOptions as option}
							<label
								class="option-card"
								class:selected={currentAnswer === option.valor}
								on:click={() => selectAnswer(option.valor)}
							>
								<input
									type="radio"
									name="quiz-option-{currentQuestionIndex}"
									value={option.valor}
									checked={currentAnswer === option.valor}
								/>
								<span class="radio-custom"></span>
								<span class="option-text">{option.texto}</span>
							</label>
						{/each}
					</div>
					<div class="navigation-buttons">
						<button
							class="btn btn-outline"
							id="btn-anterior"
                            style="
                                display: flex;
                                align-items: center;
                                justify-content: center;
                                gap: 8px;
                                text-align: center;
                            "
							on:click={handleQuizPrevious}
							disabled={currentQuestionIndex === 0}
						>
							<i class="fas fa-arrow-left"></i>
							Anterior
						</button>
						<button
							class="btn btn-principal"
							id="btn-proxima"
                            style="
                                display: flex;
                                align-items: center;
                                justify-content: center;
                                gap: 8px;
                                text-align: center;
                            "
							on:click={handleQuizNext}
							disabled={currentAnswer === undefined}
						>
							{#if currentQuestionIndex === questions.length - 1}
								Finalizar
								<i class="fas fa-check"></i>
							{:else}
								Próxima
								<i class="fas fa-arrow-right"></i>
							{/if}
						</button>
					</div>
				</div>

				<div class="dica-box">
					<strong>Dica:</strong> Responda com sinceridade! Não há respostas certas ou erradas. O
					objetivo é descobrir o que realmente combina com você.
				</div>
			{:else}
				<div class="loading-container">
					<h2>Oops!</h2>
					<p>Não foi possível carregar as perguntas no momento. Tente novamente mais tarde.</p>
				</div>
			{/if}
		</div>
	</div>
	{:else if pageState === 'perfil'}
	<div class="perfil-page-container">
		{#if showCelebration}
			<div class="celebration">
				{#each Array(80) as _, i} <div
						class="particle"
						style="
                        background-color: hsl({Math.random() * 360}, 100%, {60 + Math.random() * 20}%);
                        left: {50 + (Math.random() - 0.5) * 30}%; /* Começam mais centralizados */
                        top: {50 + (Math.random() - 0.5) * 30}%;
                        --x: {(Math.random() - 0.5) * 1200}px; /* Dispersão ainda maior */
                        --y: {(Math.random() - 0.5) * 1200}px;
                        animation-delay: {Math.random() * 0.5}s; /* Atraso levemente maior */
                    "
					></div>
				{/each}
			</div>
		{/if}

		{#if isLoadingProfile}
			<div class="loading-container">
				<div class="spinner"></div>
				<h2>Analisando seu perfil...</h2>
				<p>Isso pode levar alguns segundos.</p>
			</div>
		{:else if rankingData && rankingData.length > 0}
			<header class="header-perfil">
				<div class="header-perfil-info">
					<div class="avatar">{user.name ? user.name.slice(0, 2).toUpperCase() : 'JD'}</div>
					<div class="user-details">
						<h2>{user.name || 'João da Silva'}</h2>
						<p>{user.email || 'joao.silva@email.com'}</p>
					</div>
				</div>
			</header>
			<section class="perfil-card">
				<div class="card-header">
					<i class="fas fa-award icon"></i>
					<h3>Seu Ranking de Áreas</h3>
				</div>
				<p class="card-subtitle">Áreas com maior compatibilidade com seu perfil</p>

				<div class="carreiras-ranking-list">
					{#each rankingData as item, i}
						{@const details = rankingAreaDetailsMap[item.area] || defaultRankingDetails}
						<div class="carreira-ranked-item">
							<span class="rank-number">{i + 1}</span>
                            <i class="{details.icon} ranking-icon"></i>
							<div class="carreira-info">
								<h4>{item.area}</h4>
                                <p>{details.description}</p>
							</div>
							<span class="tag tag-blue">Score: {item.score}</span>
						</div>
					{/each}
				</div>
			</section>
		{:else}
			<div class="loading-container">
				<h2>Oops!</h2>
				<p>Não foi possível carregar seus resultados no momento. Tente novamente mais tarde.</p>
			</div>
		{/if}
	</div>
{/if}