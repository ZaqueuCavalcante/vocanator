<script lang="ts">
	import { PUBLIC_API_URL } from '$env/static/public';
	import { onMount } from 'svelte';

	// --- ESTADO GLOBAL DA PÁGINA ---
	let pageState = $state<'home' | 'quiz' | 'perfil'>('home');
	let modalVisible = $state(true);

	// --- DADOS DO USUÁRIO ---
	let user = $state({ name: '', email: '' });
	let userNameInput = $state('');
	let userEmailInput = $state('');

	// --- DADOS DA API (APENAS ÁREAS) ---
	let areas = $state<any[]>([]);

	// --- DADOS ESTÁTICOS DO QUIZ ---
	const quizQuestions = [
		{
			text: 'Você gosta de resolver quebra-cabeças ou problemas lógicos?',
			badge: 'Interesses',
			intelligence: 'logico'
		},
		{
			text: 'Você se sente à vontade apresentando ideias para um grupo?',
			badge: 'Comportamento',
			intelligence: 'linguistico'
		},
		{
			text: 'Você prefere garantir que todos no grupo estejam felizes e trabalhando juntos?',
			badge: 'Comportamento',
			intelligence: 'interpessoal'
		},
		{
			text: 'Você gosta de construir protótipos ou modelos físicos?',
			badge: 'Comportamento',
			intelligence: 'corporal'
		},
		{
			text: 'Um laboratório de pesquisa silencioso parece um bom lugar para trabalhar?',
			badge: 'Ambiente',
			intelligence: 'intrapessoal'
		},
		{
			text: 'Você se sente atraído por design, arquitetura e artes visuais?',
			badge: 'Ambiente',
			intelligence: 'espacial'
		},
		{
			text: 'Você prefere trabalhar ao ar livre, em contato com a natureza?',
			badge: 'Ambiente',
			intelligence: 'naturalista'
		},
		{
			text: 'Você consegue perceber facilmente se uma nota musical está desafinada?',
			badge: 'Habilidade',
			intelligence: 'musical'
		},
		{
			text: 'Você aprende melhor lendo e escrevendo anotações?',
			badge: 'Aprendizado',
			intelligence: 'linguistico'
		},
		{
			text: 'Você aprende melhor "colocando a mão na massa" e fazendo na prática?',
			badge: 'Aprendizado',
			intelligence: 'corporal'
		}
	];
	
	// --- OPÇÕES ATUALIZADAS PARA SIM/NÃO ---
	const quizOptions = [
		{ texto: 'Sim', valor: 5 }, // Pontuação alta para "Sim"
		{ texto: 'Não', valor: 1 }  // Pontuação baixa para "Não"
	];

	// --- ESTADO DO QUIZ ---
	let questions = $state(quizQuestions);
	let currentQuestionIndex = $state(0);
	let answers = $state<Record<number, number>>({}); // Armazena a pontuação (5 ou 1)
	let isLoadingProfile = $state(false);
	let profileData = $state<any>(null);

	// --- DADOS COMPUTADOS (DERIVED) ---
	let currentQuestion = $derived(questions[currentQuestionIndex]);
	let currentAnswer = $derived(answers[currentQuestionIndex]);
	let quizProgress = $derived(
		questions.length > 0 ? ((currentQuestionIndex + 1) / questions.length) * 100 : 0
	);

	// --- MAPA LOCAL PARA COMPLEMENTAR DADOS DA API ---
	const areaDetailsMap: Record<string, { icon: string; description: string }> = {
		'Lógico-matemática': {
			icon: 'fas fa-calculator',
			description: 'Habilidade com números, lógica, resolução de problemas e raciocínio.'
		},
		Linguística: {
			icon: 'fas fa-comments',
			description: 'Habilidade com palavras, comunicação verbal e escrita. Facilidade em se expressar.'
		},
		Espacial: {
			icon: 'fas fa-cubes',
			description: 'Capacidade de visualizar e manipular objetos mentalmente. Importante para artes e design.'
		},
		Musical: {
			icon: 'fas fa-music',
			description: 'Sensibilidade para ritmo, melodia e sons. Facilidade em compor e tocar.'
		},
		'Corporal-cinestésica': {
			icon: 'fas fa-person-running',
			description: 'Habilidade em usar o corpo, coordenação motora e expressão física.'
		},
		Interpessoal: {
			icon: 'fas fa-users',
			description: 'Entendimento sobre os outros, empatia, capacidade de se relacionar e liderar.'
		},
		Intrapessoal: {
			icon: 'fas fa-brain',
			description: 'Autoconhecimento, capacidade de reflexão e entendimento das próprias emoções.'
		},
		Naturalista: {
			icon: 'fas fa-leaf',
			description: 'Sensibilidade para compreender e organizar padrões da natureza e meio ambiente.'
		}
	};

	// --- CARREGAMENTO DE DADOS ---
	async function loadData() {
		try {
			const resAreas = await fetch(`${PUBLIC_API_URL}/knowledge-areas`);
			if (resAreas.ok) {
				const data = await resAreas.json();
				areas = data.map((area: any) => ({
					...area,
					...areaDetailsMap[area.name]
				}));
			}
		} catch (error) {
			console.error('Falha ao carregar dados:', error);
		}
	}

	onMount(loadData);

	// --- NAVEGAÇÃO E AÇÕES ---
	async function handleRegistration() {
		// TODO: Implementar chamada de API de registro
		user.name = userNameInput;
		user.email = userEmailInput;
		modalVisible = false;
	}

	function startQuiz() {
		pageState = 'quiz';
		currentQuestionIndex = 0;
		answers = {};
	}

	function selectAnswer(value: number) {
		answers[currentQuestionIndex] = value;
	}

	function handleQuizNext() {
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

		const quizPayload = questions.map((question, index) => ({
			question: question.text,
			intelligence: question.intelligence,
			score: answers[index] || 1 // Envia a pontuação (5 ou 1)
		}));

		console.log('Enviando para o backend:', quizPayload);

		// TODO: Implementar chamada real ao backend com o 'quizPayload'
		// const res = await fetch(`${PUBLIC_API_URL}/api/analyze-quiz`, {
		//     method: 'POST',
		//     headers: { 'Content-Type': 'application/json' },
		//     body: JSON.stringify(quizPayload)
		// });
		// if (!res.ok) { ... }
		// profileData = await res.json();

		// Simulação de delay de rede
		setTimeout(() => {
			profileData = {
				perfilPrincipal: {
					nome: 'Analítico',
					descricao: 'Você tem forte capacidade de raciocínio lógico e resolução de problemas complexos.'
				},
				inteligencias: [
					{ nome: 'Lógico-Matemática' },
					{ nome: 'Intrapessoal' },
					{ nome: 'Linguística' }
				],
				carreiras: [
					{
						nome: 'Engenharia de Software',
						descricao: 'Desenvolvimento de sistemas e aplicações tecnológicas',
						compatibilidade: 95
					},
					{
						nome: 'Ciência de Dados',
						descricao: 'Análise e interpretação de grandes volumes de dados',
						compatibilidade: 92
					},
					{
						nome: 'Pesquisa Científica',
						descricao: 'Investigação e desenvolvimento em áreas científicas',
						compatibilidade: 88
					}
				]
			};
			isLoadingProfile = false;
		}, 2000);
	}
</script>

<style>
	* {
		box-sizing: border-box;
		margin: 0;
		padding: 0;
	}

	body {
		font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial,
			sans-serif;
		background-color: #f9f9f9;
		color: #333;
		line-height: 1.6;
	}

	.btn {
		display: inline-flex;
		align-items: center;
		gap: 8px;
		padding: 14px 32px;
		border-radius: 8px;
		text-decoration: none;
		font-weight: bold;
		font-size: 1.1em;
		cursor: pointer;
		border: 1px solid transparent;
		transition: all 0.2s ease;
	}
	.btn:hover {
		transform: translateY(-2px);
	}
	.btn-principal {
		background-color: #007bff;
		color: #ffffff;
		border: none;
	}
	.btn-principal[disabled] {
		background-color: #a0cfff;
		cursor: not-allowed;
	}
	.btn-principal i {
		margin-left: 8px;
	}

	.btn-outline {
		background-color: transparent;
		color: #555;
		border-color: #ccc;
		padding: 10px 20px;
		font-size: 1em;
		font-weight: 600;
	}
	.btn-outline:hover {
		background-color: #f0f0f0;
	}
	.btn-outline:disabled {
		color: #ccc;
		border-color: #eee;
		cursor: not-allowed;
	}

	.section-subtitle {
		font-size: 1.1em;
		color: #555;
		max-width: 700px;
		margin: 0 auto 40px auto;
		text-align: center;
	}

	.tag {
		display: inline-block;
		padding: 4px 12px;
		border-radius: 20px;
		font-size: 0.9em;
		font-weight: 500;
		flex-shrink: 0;
	}
	.tag-blue {
		background-color: #e6f2ff;
		color: #007bff;
	}
	.tag-gray {
		background-color: #f0f0f0;
		color: #555;
		border: 1px solid #ddd;
	}

	.modal-overlay {
		position: fixed;
		top: 0;
		left: 0;
		right: 0;
		bottom: 0;
		background-color: rgba(0, 0, 0, 0.6);
		display: flex;
		justify-content: center;
		align-items: center;
		z-index: 2000;
		padding: 20px;
	}
	.modal-content {
		background-color: #ffffff;
		padding: 30px;
		border-radius: 12px;
		width: 100%;
		max-width: 400px;
		text-align: center;
		position: relative;
	}
	.modal-content h2 {
		font-size: 1.8em;
		color: #2c3e50;
		margin-bottom: 10px;
	}
	.modal-content p {
		font-size: 1em;
		color: #555;
		margin-bottom: 25px;
	}
	.form-grupo {
		margin-bottom: 15px;
		text-align: left;
	}
	.form-grupo label {
		display: block;
		font-weight: 600;
		margin-bottom: 5px;
		color: #333;
	}
	.form-grupo input {
		width: 100%;
		padding: 12px;
		font-size: 1em;
		border: 1px solid #ccc;
		border-radius: 8px;
	}
	.form-grupo input:focus {
		outline: none;
		border-color: #007bff;
		box-shadow: 0 0 5px rgba(0, 123, 255, 0.3);
	}
	.modal-content .btn-principal {
		width: 100%;
	}

	.navbar {
		display: flex;
		justify-content: space-between;
		align-items: center;
		padding: 15px 25px;
		background-color: #ffffff;
		border-bottom: 1px solid #e0e0e0;
	}
	.logo {
		font-size: 1.8em;
		font-weight: bold;
		color: #007bff;
		text-decoration: none;
	}
	.perfil-link {
		text-decoration: none;
		color: #333;
		font-weight: 600;
		font-size: 1em;
	}
	.perfil-link i {
		margin-right: 8px;
		color: #007bff;
	}

	.hero-section {
		text-align: center;
		padding: 80px 20px;
		background-color: #ffffff;
	}
	.hero-badge {
		display: inline-block;
		padding: 8px 15px;
		border-radius: 20px;
		background-color: #e6f2ff;
		color: #007bff;
		font-size: 0.9em;
		font-weight: 600;
		margin-bottom: 25px;
	}
	.hero-badge i {
		margin-right: 6px;
	}
	.hero-section h1 {
		font-size: 2.8em;
		font-weight: 700;
		color: #222;
		max-width: 650px;
		margin: 0 auto 20px auto;
		line-height: 1.2;
	}
	.hero-section p {
		font-size: 1.1em;
		color: #555;
		max-width: 600px;
		margin: 0 auto 35px auto;
	}

	.como-funciona-section {
		padding: 60px 20px;
		background-color: #f9f9f9;
	}
	.como-funciona-section h2 {
		font-size: 2em;
		color: #2c3e50;
		margin-bottom: 15px;
		text-align: center;
	}
	.passos-container {
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
		gap: 20px;
		max-width: 1200px;
		margin: 0 auto;
	}
	.passo-card {
		background-color: #ffffff;
		border: 1px solid #e0e0e0;
		border-radius: 12px;
		padding: 30px;
		text-align: left;
	}
	.passo-card.highlight {
		border-color: #007bff;
		box-shadow: 0 4px 15px rgba(0, 123, 255, 0.1);
	}
	.passo-icon {
		font-size: 1.5em;
		color: #007bff;
		background-color: #e6f2ff;
		width: 45px;
		height: 45px;
		border-radius: 50%;
		display: flex;
		align-items: center;
		justify-content: center;
		margin-bottom: 20px;
	}
	.passo-card h3 {
		font-size: 1.2em;
		font-weight: 600;
		color: #333;
		margin-bottom: 10px;
	}
	.passo-card p {
		font-size: 0.95em;
		color: #555;
		line-height: 1.5;
	}

	.inteligencias-section {
		padding: 60px 20px;
		background-color: #ffffff;
	}
	.inteligencias-section h2 {
		font-size: 2.2em;
		color: #2c3e50;
		margin-bottom: 15px;
		text-align: center;
	}
	.inteligencias-grid {
		display: grid;
		grid-template-columns: repeat(auto-fit, minmax(280px, 1fr));
		gap: 20px;
		max-width: 1200px;
		margin: 40px auto 0 auto;
	}
	.inteligencia-card {
		background-color: #ffffff;
		border: 1px solid #e0e0e0;
		border-radius: 12px;
		padding: 25px;
		text-align: left;
	}
	.inteligencia-card .icon {
		font-size: 2em;
		color: #007bff;
		margin-bottom: 15px;
	}
	.inteligencia-card h5 {
		font-size: 1.2em;
		color: #333;
		margin-bottom: 5px;
		font-weight: 600;
	}
	.inteligencia-card p {
		font-size: 0.95em;
		color: #555;
		line-height: 1.5;
	}

	.footer {
		background-color: #2c3e50;
		color: #bdc3c7;
		padding: 50px 20px 20px 20px;
		text-align: left;
	}
	.footer-container {
		display: flex;
		flex-wrap: wrap;
		justify-content: space-between;
		max-width: 1200px;
		margin: 0 auto;
		gap: 30px;
	}
	.footer-col {
		flex: 1;
		min-width: 200px;
	}
	.footer-col h6 {
		font-size: 1em;
		color: #ffffff;
		margin-bottom: 15px;
	}
	.footer-col ul {
		list-style: none;
	}
	.footer-col ul li {
		margin-bottom: 8px;
	}
	.footer-col ul li a {
		color: #bdc3c7;
		text-decoration: none;
	}
	.footer-col ul li a:hover {
		color: #ffffff;
	}
	.copyright {
		text-align: center;
		margin-top: 40px;
		padding-top: 20px;
		border-top: 1px solid #4e5d6c;
		font-size: 0.9em;
		max-width: 1200px;
		margin-left: auto;
		margin-right: auto;
	}

	.quiz-page-container {
		display: flex;
		justify-content: center;
		align-items: center;
		min-height: 100vh;
		padding: 20px;
		background-color: #f4f7f6;
	}
	.quiz-container {
		width: 100%;
		max-width: 550px;
	}
	.progress-header {
		display: flex;
		justify-content: space-between;
		align-items: center;
		margin-bottom: 8px;
		font-size: 0.9em;
		font-weight: 600;
	}
	#pergunta-atual {
		color: #333;
	}
	#porcentagem-completa {
		color: #007bff;
	}
	.progress-bar-container {
		width: 100%;
		height: 8px;
		background-color: #e0e0e0;
		border-radius: 4px;
		margin-bottom: 20px;
		overflow: hidden;
	}
	.progress-bar-fill {
		width: 10%;
		height: 100%;
		background-color: #007bff;
		border-radius: 4px;
		transition: width 0.4s ease;
	}
	.quiz-card {
		background-color: #ffffff;
		border: 1px solid #e0e0e0;
		border-radius: 12px;
		padding: 30px;
		box-shadow: 0 4px 15px rgba(0, 0, 0, 0.05);
	}
	.quiz-badge {
		display: inline-block;
		padding: 6px 12px;
		border-radius: 20px;
		background-color: #e6f2ff;
		color: #007bff;
		font-size: 0.9em;
		font-weight: 600;
		margin-bottom: 15px;
	}
	.quiz-card h2 {
		font-size: 1.6em;
		font-weight: 700;
		color: #222;
		margin-bottom: 25px;
	}
	.options-container {
		display: flex;
		flex-direction: column;
		gap: 12px;
	}
	.option-card {
		display: flex;
		align-items: center;
		padding: 15px;
		border: 2px solid #e0e0e0;
		border-radius: 10px;
		cursor: pointer;
		transition: border-color 0.2s ease, background-color 0.2s ease;
	}
	.option-card:hover {
		background-color: #f9f9f9;
	}
	.option-card.selected {
		border-color: #007bff;
		background-color: #f7fbff;
	}
	.option-card input[type='radio'] {
		display: none;
	}
	.radio-custom {
		width: 22px;
		height: 22px;
		border: 2px solid #ccc;
		border-radius: 50%;
		margin-right: 15px;
		display: flex;
		align-items: center;
		justify-content: center;
		transition: border-color 0.2s ease;
		flex-shrink: 0;
	}
	.option-card.selected .radio-custom {
		border-color: #007bff;
	}
	.radio-custom::after {
		content: '';
		width: 12px;
		height: 12px;
		background-color: #007bff;
		border-radius: 50%;
		transform: scale(0);
		transition: transform 0.2s ease;
	}
	.option-card.selected .radio-custom::after {
		transform: scale(1);
	}
	.option-text {
		font-size: 1em;
		font-weight: 500;
		color: #333;
	}
	.navigation-buttons {
		display: flex;
		justify-content: space-between;
		align-items: center;
		margin-top: 30px;
		border-top: 1px solid #f0f0f0;
		padding-top: 20px;
	}
	.dica-box {
		background-color: #e6f2ff;
		border: 1px solid #b3d7ff;
		border-radius: 10px;
		padding: 15px;
		margin-top: 20px;
		text-align: center;
		font-size: 0.9em;
		color: #333;
	}
	.dica-box strong {
		color: #0056b3;
	}

	.perfil-page-container {
		max-width: 1000px;
		margin: 20px auto;
		display: grid;
		gap: 20px;
	}
	.perfil-card {
		background-color: #ffffff;
		border: 1px solid #e0e0e0;
		border-radius: 12px;
		padding: 25px;
		box-shadow: 0 2px 8px rgba(0, 0, 0, 0.03);
	}
	.header-perfil {
		display: flex;
		justify-content: space-between;
		align-items: center;
		background-color: #ffffff;
		padding: 20px;
		border-radius: 12px;
	}
	.header-perfil-info {
		display: flex;
		align-items: center;
		gap: 15px;
	}
	.avatar {
		width: 50px;
		height: 50px;
		border-radius: 50%;
		background-color: #e6f2ff;
		color: #007bff;
		display: flex;
		align-items: center;
		justify-content: center;
		font-size: 1.2em;
		font-weight: bold;
		flex-shrink: 0;
	}
	.user-details h2 {
		font-size: 1.3em;
		font-weight: 600;
		color: #222;
		margin: 0;
	}
	.user-details p {
		font-size: 0.95em;
		color: #555;
		margin: 2px 0 0 0;
		line-height: 1.4;
	}
	.user-details p i {
		margin-right: 5px;
		color: #777;
	}
	.cards-meio {
		display: grid;
		grid-template-columns: 1fr;
		gap: 20px;
	}
	.card-header {
		display: flex;
		align-items: center;
		gap: 10px;
		margin-bottom: 5px;
	}
	.card-header .icon {
		font-size: 1.1em;
		color: #007bff;
	}
	.card-header h3 {
		font-size: 1.1em;
		color: #2c3e50;
		margin: 0;
		font-weight: 600;
	}
	.card-subtitle {
		font-size: 0.9em;
		color: #777;
		margin: -5px 0 15px 0;
	}
	.card-perfil-item {
		display: flex;
		justify-content: space-between;
		align-items: flex-start;
		margin-bottom: 15px;
		padding-bottom: 15px;
		border-bottom: 1px solid #f0f0f0;
	}
	.card-perfil-item:last-child {
		margin-bottom: 0;
		padding-bottom: 0;
		border-bottom: none;
	}
	.card-perfil-item h4 {
		font-size: 0.95em;
		font-weight: 600;
		color: #333;
		margin-bottom: 3px;
	}
	.card-perfil-item p {
		font-size: 0.9em;
		color: #555;
		max-width: 250px;
	}
	.tags-container {
		display: flex;
		gap: 8px;
		flex-wrap: wrap;
	}
	.carreiras-ranking-list {
		display: flex;
		flex-direction: column;
		gap: 15px;
		margin-top: 15px;
	}
	.carreira-ranked-item {
		display: flex;
		align-items: center;
		gap: 20px;
		background-color: #fdfdfd;
		border: 1px solid #e8e8e8;
		border-radius: 10px;
		padding: 20px;
	}
	.rank-number {
		font-size: 1.8em;
		font-weight: 700;
		color: #007bff;
		min-width: 30px;
		text-align: center;
	}
	.carreira-info {
		flex-grow: 1;
	}
	.carreira-info h4 {
		font-size: 1.1em;
		font-weight: 600;
		color: #333;
		margin-bottom: 5px;
	}
	.carreira-info p {
		font-size: 0.9em;
		color: #555;
		margin-bottom: 0;
	}
	.carreira-ranked-item .tag-blue {
		font-size: 0.85em;
		font-weight: 600;
	}

	.loading-container {
		display: flex;
		flex-direction: column;
		align-items: center;
		justify-content: center;
		padding: 100px 20px;
		text-align: center;
	}
	.loading-container h2 {
		font-size: 1.5em;
		color: #333;
		margin-top: 20px;
	}
	.spinner {
		border: 4px solid #f3f3f3;
		border-top: 4px solid #007bff;
		border-radius: 50%;
		width: 40px;
		height: 40px;
		animation: spin 1s linear infinite;
	}
	@keyframes spin {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
	}

	@media (max-width: 768px) {
		.hero-section h1 {
			font-size: 2.2em;
		}
		.como-funciona-section h2,
		.inteligencias-section h2 {
			font-size: 1.8em;
		}
		.inteligencias-grid {
			grid-template-columns: 1fr;
		}
		.passos-container {
			grid-template-columns: 1fr;
		}
		.logo {
			font-size: 1.5em;
		}
		.perfil-link {
			font-size: 0.9em;
		}
	}

	@media (max-width: 600px) {
		.header-perfil {
			flex-direction: column;
			align-items: flex-start;
			gap: 15px;
		}
		.carreira-ranked-item {
			flex-direction: column;
			align-items: flex-start;
			gap: 10px;
		}
		.carreira-info {
			width: 100%;
		}
		.carreira-ranked-item .tag-blue {
			margin-top: 10px;
		}
	}
</style>

{#if pageState === 'home'}
	{#if modalVisible}
		<div class="modal-overlay" id="modal-cadastro">
			<div class="modal-content">
				<h2>Bem-vindo!</h2>
				<p>Cadastre-se para descobrir sua vocação e salvar seu progresso.</p>
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
					<button type="submit" class="btn btn-principal">Cadastrar e Iniciar</button>
				</form>
			</div>
		</div>
	{/if}

	<div id="pagina-principal">
		<header class="navbar">
			<a href="#" class="logo">Vocanator</a>
			<a href="#" class="perfil-link" id="perfil-link">
				<i class="fas fa-user-circle"></i>
				<span>{user.name ? `Olá, ${user.name.split(' ')[0]}` : 'Seu Perfil'}</span>
			</a>
		</header>

		<main>
			<section class="hero-section">
				<span class="hero-badge">
					<i class="fas fa-atom"></i>
					Descubra seu futuro profissional
				</span>
				<h1>Descubra sua verdadeira vocação e encontre a carreira que combina com você</h1>
				<p>
					Use inteligência artificial e testes gamificados para descobrir suas habilidades, interesses e
					o caminho profissional perfeito para você.
				</p>
				<button class="btn btn-principal" on:click={startQuiz}>
					Comece agora
					<i class="fas fa-arrow-right"></i>
				</button>
			</section>

			<section class="como-funciona-section">
				<h2>Como funciona</h2>
				<p class="section-subtitle">Três passos simples para descobrir sua vocação profissional</p>
				<div class="passos-container">
					<div class="passo-card">
						<div class="passo-icon"><i class="fas fa-check-circle"></i></div>
						<h3>Responda perguntas rápidas</h3>
						<p>
							Complete nosso teste vocacional interativo com perguntas sobre seus interesses,
							habilidades e personalidade.
						</p>
					</div>
					<div class="passo-card highlight">
						<div class="passo-icon"><i class="fas fa-bullseye"></i></div>
						<h3>Descubra seu perfil vocacional</h3>
						<p>
							Nossa IA analisa suas respostas e identifica seu perfil único: Analítico, Criativo,
							Empreendedor ou Social.
						</p>
					</div>
					<div class="passo-card">
						<div class="passo-icon"><i class="fas fa-chart-line"></i></div>
						<h3>Receba sugestões personalizadas</h3>
						<p>
							Obtenha recomendações de carreiras, cursos e áreas de estudo que combinam
							perfeitamente com você.
						</p>
					</div>
				</div>
			</section>

			<section class="inteligencias-section">
				<h2>Teoria das Inteligências Múltiplas</h2>
				<p class="section-subtitle">
					Desenvolvida por Howard Gardner, a teoria das Inteligências Múltiplas revolucionou nossa
					compreensão sobre as capacidades humanas. Gardner propôs que não existe apenas um tipo de
					inteligência, mas sim múltiplas formas de ser inteligente. Nosso teste vocacional é
					baseado nesta teoria científica para identificar suas habilidades únicas.
				</p>

				<div class="inteligencias-grid">
					{#each areas as area}
						<div class="inteligencia-card">
							<i class="{area.icon || 'fas fa-star'} icon"></i>
							<h5>{area.name}</h5>
							<p>{area.description || 'Descrição indisponível.'}</p>
						</div>
					{:else}
						<p>Carregando áreas do conhecimento...</p>
					{/each}
				</div>
			</section>
		</main>

		<footer class="footer">
			<div class="footer-container">
				<div class="footer-col">
					<h6>Vocanator</h6>
					<p style="font-size: 0.9em;">Descubra sua verdadeira vocação.</p>
				</div>
				<div class="footer-col">
					<h6>Plataforma</h6>
					<ul>
						<li><a href="#" on:click={startQuiz}>Fazer teste</a></li>
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
			{#if questions.length > 0 && currentQuestion}
				<div class="progress-header">
					<span id="pergunta-atual">Pergunta {currentQuestionIndex + 1} de {questions.length}</span>
					<span id="porcentagem-completa">{Math.round(quizProgress)}% completo</span>
				</div>
				<div class="progress-bar-container">
					<div class="progress-bar-fill" id="progress-bar-fill" style="width: {quizProgress}%"></div>
				</div>

				<div class="quiz-card">
					<span class="quiz-badge" id="quiz-badge">{currentQuestion.badge || 'Pergunta'}</span>
					<h2 id="question-text">{currentQuestion.text}</h2>

					<div class="options-container" id="options-container">
						{#each quizOptions as option}
							<label
								class="option-card"
								class:selected={currentAnswer === option.valor}
								on:click={() => selectAnswer(option.valor)}
							>
								<input type="radio" name="quiz-option" value={option.valor} />
								<span class="radio-custom"></span>
								<span class="option-text">{option.texto}</span>
							</label>
						{/each}
					</div>

					<div class="navigation-buttons">
						<button
							class="btn btn-outline"
							id="btn-anterior"
							on:click={handleQuizPrevious}
							disabled={currentQuestionIndex === 0}
						>
							<i class="fas fa-arrow-left"></i>
							Anterior
						</button>
						<button
							class="btn btn-principal"
							id="btn-proxima"
							on:click={handleQuizNext}
							disabled={!currentAnswer}
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
					<div class="spinner"></div>
					<h2>Carregando perguntas...</h2>
				</div>
			{/if}
		</div>
	</div>
	{:else if pageState === 'perfil'}
	<div class="perfil-page-container">
		{#if isLoadingProfile}
			<div class="loading-container">
				<div class="spinner"></div>
				<h2>Analisando seu perfil...</h2>
				<p>Isso pode levar alguns segundos.</p>
			</div>
		{:else if profileData}
			<header class="header-perfil">
				<div class="header-perfil-info">
					<div class="avatar">{user.name ? user.name.slice(0, 2).toUpperCase() : 'JD'}</div>
					<div class="user-details">
						<h2>{user.name || 'João da Silva'}</h2>
						<p>{user.email || 'joao.silva@email.com'}</p>
					</div>
				</div>
			</header>

			<div class="cards-meio">
				<section class="perfil-card">
					<div class="card-header">
						<i class="fas fa-user-check icon"></i>
						<h3>Perfil Vocacional</h3>
					</div>
					<p class="card-subtitle">Seu perfil identificado pelo teste</p>

					<div class="card-perfil-item">
						<div>
							<h4>Perfil Principal</h4>
							<p>{profileData.perfilPrincipal.descricao}</p>
						</div>
						<span class="tag tag-blue">{profileData.perfilPrincipal.nome}</span>
					</div>

					<div class="card-perfil-item">
						<div>
							<h4>Inteligências Dominantes</h4>
						</div>
						<div class="tags-container">
							{#each profileData.inteligencias as inteligencia}
								<span class="tag tag-gray">{inteligencia.nome}</span>
							{/each}
						</div>
					</div>
				</section>
			</div>

			<section class="perfil-card">
				<div class="card-header">
					<i class="fas fa-chart-line icon"></i>
					<h3>Carreiras Recomendadas</h3>
				</div>
				<p class="card-subtitle">Baseado no seu perfil vocacional</p>

				<div class="carreiras-ranking-list">
					{#each profileData.carreiras as carreira, i}
						<div class="carreira-ranked-item">
							<span class="rank-number">{i + 1}</span>
							<div class="carreira-info">
								<h4>{carreira.nome}</h4>
								<p>{carreira.descricao}</p>
							</div>
							<span class="tag tag-blue">{carreira.compatibilidade}% compatível</span>
						</div>
					{/each}
				</div>
			</section>
		{/if}
	</div>
{/if}