const favButtons = document.querySelectorAll(".btn-fav-toggle");

favButtons.forEach((btn) => {
  btn.addEventListener("click", function (e) {
    e.preventDefault();

    // Encontra o card pai
    const card = this.closest(".tech-card");
    const techName = card.querySelector(".tech-name").innerText;

    if (confirm(`Deseja remover ${techName} dos seus favoritos?`)) {
      // Efeito visual de saída
      card.style.transition = "all 0.4s ease";
      card.style.opacity = "0";
      card.style.transform = "scale(0.9)";

      // Remove o elemento do HTML após a animação
      setTimeout(() => {
        card.remove();

        // Verifica se a lista ficou vazia
        const grid = document.querySelector(".tech-grid");
        if (grid && grid.querySelectorAll(".tech-card").length === 0) {
          grid.innerHTML =
            '<p style="text-align:center; grid-column: 1/-1; color: var(--text-light); margin-top: 50px;">Você ainda não tem técnicos favoritos.</p>';
        }
      }, 400);

      alert(`${techName} removido com sucesso.`);
    }
  });
});
// ==========================================
// REDIRECIONAMENTO DOS FAVORITOS PARA AGENDAR
// ==========================================
const agendarFavBtns = document.querySelectorAll(".btn-agendar-fav");

agendarFavBtns.forEach((btn) => {
  btn.addEventListener("click", function () {
    // Encontra o card onde o botão foi clicado
    const card = this.closest(".tech-card");

    // Pega os dados do técnico deste card
    const name = card.querySelector(".tech-name").innerText;
    const imgSrc = card.querySelector(".tech-avatar").getAttribute("src");

    // Salva no localStorage para a tela de chat/agendar usar
    localStorage.setItem("techName", name.trim());
    localStorage.setItem("techImg", imgSrc);

    // Redireciona
    window.location.href = "agendar-reparo.html";
  });
});
