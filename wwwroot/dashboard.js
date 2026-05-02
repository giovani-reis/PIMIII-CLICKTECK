// LÓGICA DE FILTROS
const filterBtns = document.querySelectorAll(".filter-btn");
const techCards = document.querySelectorAll(".tech-card");

filterBtns.forEach((btn) => {
  btn.addEventListener("click", () => {
    // Remove a classe 'active' de todos os botões
    filterBtns.forEach((b) => b.classList.remove("active"));

    // Adiciona a classe 'active' no botão clicado
    btn.classList.add("active");

    // Pega a categoria escolhida
    const filterValue = btn.getAttribute("data-filter");

    // Esconde ou mostra os cards
    techCards.forEach((card) => {
      const categories = card.getAttribute("data-category");
      if (filterValue === "todos" || categories.includes(filterValue)) {
        card.style.display = "block";
      } else {
        card.style.display = "none";
      }
    });
  });
});

const modal = document.getElementById("profile-modal");
const closeBtn = document.querySelector(".close-modal");
const viewBtns = document.querySelectorAll(".view-profile-btn");

// Elementos de dentro do modal para preencher os dados
const modalName = document.getElementById("modal-name");
const modalRole = document.getElementById("modal-role");
const modalImg = document.getElementById("modal-img");
const modalDesc = document.getElementById("modal-desc"); 
viewBtns.forEach((btn) => {
  btn.addEventListener("click", (e) => {
    // Acha o card "pai" do botão clicado
    const card = e.target.closest(".tech-card");

    // Pega as informações do HTML do card
    const name = card.querySelector(".tech-name").innerText;
    const role = card.querySelector(".tech-role").innerText;
    const avatarElement = card.querySelector(".tech-avatar");
    const imgSrc = avatarElement ? avatarElement.getAttribute("src") : "";

    // NOVO: Pega a descrição exclusiva do técnico
    const description = card.getAttribute("data-description");

    // Preenche o modal
    if (modalName) modalName.innerText = name;
    if (modalRole) modalRole.innerText = role;
    if (modalDesc) modalDesc.innerText = description; // Atualiza o texto dinâmico

    // Aplica a imagem no modal
    if (modalImg && imgSrc) {
      modalImg.src = imgSrc;
    }

    // Mostra o modal
    modal.classList.add("active");
  });
});

// Fechar modal no X
closeBtn.addEventListener("click", () => {
  modal.classList.remove("active");
});

// Fechar modal clicando fora dele
window.addEventListener("click", (e) => {
  if (e.target === modal) {
    modal.classList.remove("active");
  }
});


const agendarBtns = document.querySelectorAll(".tech-actions .btn-primary");
agendarBtns.forEach((btn) => {
  btn.addEventListener("click", (e) => {
    // Acha o card "pai" de onde o clique veio
    const card = e.target.closest(".tech-card");

    // Extrai o nome e a imagem (tirando ícones extras que possam ter no nome)
    let name = card.querySelector(".tech-name").innerText;
    const imgSrc = card.querySelector(".tech-avatar").src;

    // Limpa o nome (caso o SVG de verificado venha junto no innerText)
    name = name.replace(/✓/g, "").trim();

    // Salva as informações na memória do navegador (localStorage)
    localStorage.setItem("techName", name);
    localStorage.setItem("techImg", imgSrc);

    // Redireciona o usuário para a página de chat
    window.location.href = "agendar-reparo.html";
  });
});

// === NOVA LÓGICA: MENU DE LOGOFF ===
const userProfileBtn = document.getElementById("user-profile-btn");
const dropdownMenu = document.getElementById("dropdown-menu");
const btnLogoff = document.getElementById("btn-logoff");

// 1. Abrir/Fechar o menu ao clicar no "Olá, Nicolly"
userProfileBtn.addEventListener("click", (e) => {
  dropdownMenu.classList.toggle("active");
  e.stopPropagation(); // Evita que o clique se espalhe e feche o menu imediatamente
});

// 2. Fechar o menu ao clicar em qualquer outro lugar da tela
window.addEventListener("click", (e) => {
  if (!userProfileBtn.contains(e.target)) {
    dropdownMenu.classList.remove("active");
  }
});

// 3. Lógica para sair da conta ao clicar no botão "Sair"
btnLogoff.addEventListener("click", (e) => {
  e.stopPropagation();

  // Opcional: Limpa os dados do usuário salvos no navegador para simular o logoff
  localStorage.clear();

  // Redireciona para a tela inicial (ajuste "index.html" caso sua tela inicial tenha outro nome, ex: "login.html")
  window.location.href = "index.html";
});

// === LÓGICA DE NOTIFICAÇÕES ===
const notificationBtn = document.getElementById("notification-btn");
const notificationMenu = document.getElementById("notification-menu");

// 1. Abrir/Fechar as notificações ao clicar no sininho
notificationBtn.addEventListener("click", (e) => {
  notificationMenu.classList.toggle("active");

  // Esconde o menu de perfil se ele estiver aberto para não encavalar
  if (typeof dropdownMenu !== "undefined") {
    dropdownMenu.classList.remove("active");
  }

  e.stopPropagation(); // Evita que clique feche imediatamente
});

// 2. Impede que clicar dentro do menu de notificação o feche (se o usuário for rolar a lista, por exemplo)
notificationMenu.addEventListener("click", (e) => {
  e.stopPropagation();
});

// 3. Fechar as notificações ao clicar fora
window.addEventListener("click", (e) => {
  // Garante que se clicarmos em qualquer lugar fora do botão do sininho e do menu dele, ele fecha
  if (!notificationBtn.contains(e.target)) {
    notificationMenu.classList.remove("active");
  }
});


const btnMarkRead = document.querySelector(".btn-mark-read");
if (btnMarkRead) {
  btnMarkRead.addEventListener("click", () => {
    const unreadItems = document.querySelectorAll(".notif-item.unread");
    unreadItems.forEach((item) => {
      item.classList.remove("unread");
    });

    // Zera o contador visual e a bolinha vermelha
    const notifCount = document.querySelector(".notif-count");
    const badgeDot = document.querySelector(".badge-dot");

    if (notifCount) notifCount.innerText = "0 Novas";
    if (badgeDot) badgeDot.style.display = "none";
  });
}

// Seleciona todos os botões de curtir
const likeButtons = document.querySelectorAll(".btn-like");

likeButtons.forEach((button) => {
  button.addEventListener("click", function (e) {
    // Impede que o clique no coração abra o perfil (se houver link no card)
    e.stopPropagation();

    // Alterna a classe 'liked'
    this.classList.toggle("liked");

    // Pequena animação de pulso
    this.style.transform = "scale(1.3)";
    setTimeout(() => {
      this.style.transform = this.classList.contains("liked")
        ? "scale(1.1)"
        : "scale(1)";
    }, 200);
  });
});
