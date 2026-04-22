
let tareas = [];

async function descargarTaras()
{
    let response = await fetch("/taras");

    if (response.ok) {
        let datos = await response.json();

        tareas = datos;
        cosole.log(tareas);
        dibujarObjetos();
    }

}

function dibujarObjetos()
{

}


descargarTaras();




