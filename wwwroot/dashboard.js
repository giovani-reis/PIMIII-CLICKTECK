// === FUNÇÃO AUXILIAR PARA EVITAR ERROS DE ELEMENTO NULL ===
const addSafeListener = (id, event, callback) => {
    const el = document.getElementById(id);
    if (el) el.addEventListener(event, callback);
};

// === LÓGICA DE FILTROS ===
const filterBtns = document.querySelectorAll(".filter-btn");
const techCards = document.querySelectorAll(".tech-card");

filterBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
        filterBtns.forEach((b) => b.classList.remove("active"));
        btn.classList.add("active");
        const filterValue = btn.getAttribute("data-filter");
        techCards.forEach((card) => {
            const categories = card.getAttribute("data-category") || "";
            if (filterValue === "todos" || categories.toLowerCase().includes(filterValue.toLowerCase())) {
                card.style.display = "block";
            } else {
                card.style.display = "none";
            }
        });
    });
});

// === LÓGICA DO MODAL DE PERFIL ===
const profileModal = document.getElementById("profile-modal");
const closeProfileBtn = profileModal?.querySelector(".close-modal");

document.querySelectorAll(".view-profile-btn").forEach((btn) => {
    btn.addEventListener("click", () => {
        const name = btn.getAttribute("data-nome");
        const desc = btn.getAttribute("data-desc");
        document.getElementById("modal-name").innerText = name;
        document.getElementById("modal-desc").innerText = desc;
        if (profileModal) profileModal.classList.add("active");
    });
});

if (closeProfileBtn) {
    closeProfileBtn.addEventListener("click", () => profileModal.classList.remove("active"));
}

// === LÓGICA DO MODAL DE AGENDAMENTO ===
function prepararAgendamento(id, nome) {
    const modalAgendar = document.getElementById('agendar-modal');
    const inputId = document.getElementById('input-tecnico-id');
    const labelNome = document.getElementById('agendar-tecnico-nome');

    if (inputId) inputId.value = id;
    if (labelNome) labelNome.innerText = nome;
    if (modalAgendar) modalAgendar.classList.add('active');
}

// Função para fechar o agendamento (usada nos botões onclick)
function fecharAgendamento() {
    const modalAgendar = document.getElementById('agendar-modal');
    if (modalAgendar) modalAgendar.classList.remove('active');
}

// === INTERAÇÕES DE UI (DROPDOWNS E MENUS) ===
addSafeListener("user-profile-btn", "click", (e) => {
    const dropdown = document.getElementById("dropdown-menu");
    if (dropdown) dropdown.classList.toggle("active");
    e.stopPropagation();
});

addSafeListener("btn-logoff", "click", (e) => {
    e.preventDefault();
    if (confirm("Deseja realmente sair?")) {
        window.location.href = "/Account/Logout";
    }
});

addSafeListener("notification-btn", "click", (e) => {
    const notifMenu = document.getElementById("notification-menu");
    if (notifMenu) notifMenu.classList.toggle("active");
    e.stopPropagation();
});

// === GLOBAL: FECHAR TUDO AO CLICAR FORA ===
window.addEventListener("click", (e) => {
    const dropdown = document.getElementById("dropdown-menu");
    const notifMenu = document.getElementById("notification-menu");
    const modalAgendar = document.getElementById('agendar-modal');

    // Fechar Dropdowns
    if (dropdown && !document.getElementById("user-profile-btn")?.contains(e.target)) {
        dropdown.classList.remove("active");
    }
    if (notifMenu && !document.getElementById("notification-btn")?.contains(e.target)) {
        notifMenu.classList.remove("active");
    }

    // Fechar Modais ao clicar no fundo escuro (overlay)
    if (e.target === profileModal) {
        profileModal.classList.remove("active");
    }
    if (e.target === modalAgendar) {
        modalAgendar.classList.remove("active");
    }
});

// === BOTÃO DE CURTIR ===
document.querySelectorAll(".btn-like").forEach((button) => {
    button.addEventListener("click", function (e) {
        e.stopPropagation();
        this.classList.toggle("liked");
    });
});