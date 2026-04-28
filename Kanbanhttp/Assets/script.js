if (!localStorage.getItem("nombreUsuario")) {
    let nombre = prompt("¿Cual es tu nombre de usuario?");
    localStorage.setItem("nombreUsuario", nombre);
}

let tareas = [];

async function descargarTareas() {
    let response = await fetch("kanban/tareas");

    if (response.ok) {
        let datos = await response.json();

        tareas = datos;
        cosole.log(tareas);
        dibujarObjetos();
    }

}


let template = document.querySelector("template");
let columnas = document.querySelectorAll("tbody td");
let timer
function dibujarObjetos() {
    columnas.forEach(x => x.replaceChildren());

    for (let tarea of tareas) {
        let clon = template.content.cloneNode(true);

        clon.firstElementChild.children[0].innerText = tarea.Usuario;
        clon.firstElementChild.children[1].innerText = tarea.Descripcion;
        clon.firstElementChild.children[2].innerText = tarea.Fecha;
        clon.firstElementChild.dataset.id = tarea.id;
        clon.firstElementChild.dataset.usuario = tarea.Usuario;

        columnas[tarea.Estado].append(clon);

    }
    timer = setTimeout(descargarTareas(), 3000)
}



descargarTareas();

let postitMoviendo;
document.querySelector("tbody").addEventListener("dragstart", function (e) {

    if (e.target.tagName == "DIV" && (!e.target.dataset.usuario || nombre == e.target.dataset.usuario)) {
        clearTimeout(timer);
        postitMoviendo = e.target;

    }
    else {
        event.preventDefault();
    }
});

document.querySelector("tbody").addEventListener("dragover", function (e) {
    e.preventDefault();
});


document.querySelector("tbody").addEventListener("drop", async function (e) {


    if (e.target.tagName == "TD") {
        let posicionActual = postitMoviendo.parentElement.cellIndex;
        let posicionNueva = e.target.cellIndex;

        if (posicionActual + 1 == posicionNueva) {
            e.target.append(postitMoviendo);
            //Enviar al servidor
            //recargar

            let tareas = {
                id: parseInt(postitMoviendo.dataset.id),
                estado: posicionActual,
                usuario: nombre
            };

            await fetch("/kanban/movertarea", {
                method: "PUT",
                body: JSON.stringify(tareas),
                Headers: {
                    "content-type": "application/json"
                }
            });

            e.target.append(postitMoviendo);
            descargarTareas();
        }
        else {
            timer = setTimeout(descargarTareas, 3000);

        }

    }
});

