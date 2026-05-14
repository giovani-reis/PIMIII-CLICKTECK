document.addEventListener("DOMContentLoaded", () => {
    console.log("Script TechTrust carregado e pronto."); // Se isso não aparecer, o caminho do arquivo no HTML está errado.

    const filterBtns = document.querySelectorAll(".filter-btn");
    const cards = document.querySelectorAll(".appointment-card");

    // --- LÓGICA DE FILTROS ---
    filterBtns.forEach((btn) => {
        btn.onclick = function () {
            // Estilo visual dos botões
            filterBtns.forEach((b) => b.classList.remove("active"));
            this.classList.add("active");

            const filter = this.getAttribute("data-filter").toLowerCase();

            cards.forEach((card) => {
                const cardStatus = card.getAttribute("data-status")?.toLowerCase() || "";

                if (filter === "todos") {
                    card.style.display = "flex";
                }
                else if (filter === "em-aberto") {
                    const isAberto = cardStatus !== "finalizado" && cardStatus !== "cancelado";
                    card.style.display = isAberto ? "flex" : "none";
                }
                else if (filter === "concluidos") {
                    const isConcluido = cardStatus === "finalizado" || cardStatus === "cancelado";
                    card.style.display = isConcluido ? "flex" : "none";
                }
                else {
                    // Filtros específicos (solicitado, aprovado, etc)
                    card.style.display = cardStatus === filter ? "flex" : "none";
                }
            });
        };
    });

    // --- MODAL DE DETALHES ---
    const modalDet = document.getElementById("modal-detalhes");
    document.querySelectorAll(".btn-detalhes").forEach((btn) => {
        btn.onclick = () => modalDet?.classList.add("active");
    });

    const closeDet = document.getElementById("close-detalhes");
    if (closeDet) {
        closeDet.onclick = () => modalDet?.classList.remove("active");
    }

    // --- MODAL DE AVALIAÇÃO ---
    const modalEval = document.getElementById("modal-avaliacao");
    const inputAtendimentoId = document.getElementById("eval-atendimento-id");
    const techNameSpan = document.getElementById("eval-tech-name");

    document.querySelectorAll(".btn-avaliar").forEach((btn) => {
        btn.onclick = function () {
            if (techNameSpan) techNameSpan.innerText = this.getAttribute("data-tech");
            if (inputAtendimentoId) inputAtendimentoId.value = this.getAttribute("data-id");
            modalEval?.classList.add("active");
        };
    });

    const fecharAvaliacao = () => {
        modalEval?.classList.remove("active");
        document.getElementById("form-avaliacao")?.reset();
    };

    ["close-eval", "cancel-eval"].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.onclick = fecharAvaliacao;
    });
});