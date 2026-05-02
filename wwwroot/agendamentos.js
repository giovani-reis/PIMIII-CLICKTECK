// FILTROS
const filterBtns = document.querySelectorAll(".filter-btn");
const cards = document.querySelectorAll(".appointment-card");

filterBtns.forEach((btn) => {
  btn.addEventListener("click", () => {
    filterBtns.forEach((b) => b.classList.remove("active"));
    btn.classList.add("active");
    const filter = btn.getAttribute("data-filter");

    cards.forEach((card) => {
      const isDone = card.classList.contains("done");
      if (filter === "todos") card.style.display = "";
      else if (filter === "em-aberto")
        card.style.display = isDone ? "none" : "";
      else if (filter === "concluidos")
        card.style.display = isDone ? "" : "none";
    });
  });
});

// MODAL DETALHES
const modalDet = document.getElementById("modal-detalhes");
document.querySelectorAll(".btn-detalhes").forEach((btn) => {
  btn.onclick = () => modalDet.classList.add("active");
});
document.getElementById("close-detalhes").onclick = () =>
  modalDet.classList.remove("active");

// CANCELAR
document.querySelectorAll(".btn-outline-danger").forEach((btn) => {
  btn.onclick = (e) => {
    if (confirm("Deseja cancelar?")) {
      const card = e.target.closest(".appointment-card");
      card.style.opacity = "0";
      setTimeout(() => card.remove(), 300);
    }
  };
});

const modalEval = document.getElementById("modal-avaliacao");
const btnAvaliar = document.querySelectorAll(".btn-avaliar");
const btnCloseEval = document.getElementById("close-eval");
const btnCancelEval = document.getElementById("cancel-eval");
const formAvaliacao = document.getElementById("form-avaliacao");

btnAvaliar.forEach((btn) => {
  btn.addEventListener("click", () => {
    const techName = btn.getAttribute("data-tech");
    document.getElementById("eval-tech-name").innerText = techName;
    modalEval.classList.add("active");
  });
});

const fecharAvaliacao = () => {
  modalEval.classList.remove("active");
  if (formAvaliacao) formAvaliacao.reset();
};

if (btnCloseEval) btnCloseEval.onclick = fecharAvaliacao;
if (btnCancelEval) btnCancelEval.onclick = fecharAvaliacao;

if (formAvaliacao) {
  formAvaliacao.addEventListener("submit", (e) => {
    e.preventDefault();
    const nota = document.querySelector('input[name="star"]:checked');

    if (!nota) {
      alert("Por favor, selecione uma quantidade de estrelas.");
      return;
    }

    alert(
      `Obrigado! Sua avaliação de ${nota.value} estrelas foi enviada com sucesso.`,
    );
    fecharAvaliacao();
  });
}
