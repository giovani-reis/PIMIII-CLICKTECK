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
            // No MVC, garantimos que o data-category venha preenchido do banco
            const categories = card.getAttribute("data-category") || "";
            if (filterValue === "todos" || categories.toLowerCase().includes(filterValue.toLowerCase())) {
                card.style.display = "block";
            } else {
                card.style.display = "none";
            }
        });
    });
});

// === LÓGICA DO MODAL (REVISADA) ===
const modal = document.getElementById("profile-modal");
const closeBtn = document.querySelector(".close-modal");

document.querySelectorAll(".view-profile-btn").forEach((btn) => {
    btn.addEventListener("click", (e) => {
        // Agora pegamos os dados diretamente dos atributos data que colocamos no botão no C#
        const name = btn.getAttribute("data-nome");
        const desc = btn.getAttribute("data-desc");
        const img = btn.getAttribute("data-img");

        document.getElementById("modal-name").innerText = name;
        document.getElementById("modal-desc").innerText = desc;

        if (modal) modal.classList.add("active");
    });
});

if (closeBtn) {
    closeBtn.addEventListener("click", () => modal.classList.remove("active"));
}

// === MENU DE PERFIL E LOGOFF (CORREÇÃO DA LINHA 120) ===
addSafeListener("user-profile-btn", "click", (e) => {
    const dropdown = document.getElementById("dropdown-menu");
    if (dropdown) dropdown.classList.toggle("active");
    e.stopPropagation();
});

// CORREÇÃO DO LOGOFF: No MVC, recomendamos usar um formulário para Logout por segurança
addSafeListener("btn-logoff", "click", (e) => {
    e.preventDefault();
    if (confirm("Deseja realmente sair?")) {
        // Se você tiver uma Action de Logout no Controller:
        window.location.href = "/Account/Logout";
    }
});

// === NOTIFICAÇÕES ===
addSafeListener("notification-btn", "click", (e) => {
    const notifMenu = document.getElementById("notification-menu");
    if (notifMenu) notifMenu.classList.toggle("active");
    e.stopPropagation();
});

// Fechar menus ao clicar fora
window.addEventListener("click", (e) => {
    const dropdown = document.getElementById("dropdown-menu");
    const notifMenu = document.getElementById("notification-menu");

    if (dropdown && !document.getElementById("user-profile-btn").contains(e.target)) {
        dropdown.classList.remove("active");
    }
    if (notifMenu && !document.getElementById("notification-btn").contains(e.target)) {
        notifMenu.classList.remove("active");
    }
    if (e.target === modal) {
        modal.classList.remove("active");
    }
});

// === BOTÃO DE CURTIR ===
document.querySelectorAll(".btn-like").forEach((button) => {
    button.addEventListener("click", function (e) {
        e.stopPropagation();
        this.classList.toggle("liked");
    });
});

function prepararAgendamento(id, nome) {
    // 1. Coloca o ID do técnico no input hidden do formulário
    document.getElementById('input-tecnico-id').value = id;

    // 2. (Opcional) Mostra o nome dele no título do modal para o cliente ter certeza
    document.getElementById('agendar-tecnico-nome').innerText = nome;

    // 3. Abre o modal (exemplo de classe active)
    document.getElementById('agendar-modal').classList.add('active');
}