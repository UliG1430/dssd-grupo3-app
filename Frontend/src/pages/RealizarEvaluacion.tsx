import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams, useNavigate } from 'react-router-dom';
import { getOrdenesUserEvaluacion, Orden } from "../service/recoleccionService";
import { getNextTaskId, assignTask, executeTask } from "../service/bonitaService";
import { getUsuarioIdByUsername } from "../service/bonitaService";
import { updateEvaluacion } from "../service/EvaluacionService";
import { updateUltimaNotificacion } from "../service/UltimaNotificacionService";
import { getUsuarioByUsername } from "../service/UsuarioService";

const OrdenesView = () => {
  const [ordenes, setOrdenes] = useState([]);
  const [totalOrdenes, setTotalOrdenes] = useState(0);
  const [ordenesOk, setOrdenesOk] = useState(0);
  const [ordenesInv, setOrdenesInv] = useState(0);
  const [observaciones, setObservaciones] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchOrdenes = async () => {
      const caseId = localStorage.getItem("caseId");
      if (!caseId) {
        alert("No caseId found in local storage.");
        return;
      }

      try {
        setLoading(true);
        const userApp = await getUsuarioByUsername('walter.bates');
        const fetchedOrdenes = await getOrdenesUserEvaluacion(userApp.id);

        setOrdenes(fetchedOrdenes);

        // Process the ordenes
        setTotalOrdenes(fetchedOrdenes.length);
        setOrdenesOk(fetchedOrdenes.filter((o: Orden) => o.estado === "OK").length);
        setOrdenesInv(fetchedOrdenes.filter((o: Orden) => o.estado === "INV").length);
      } catch (error) {
        console.error("Error fetching ordenes:", error);
        alert("Failed to fetch ordenes.");
      } finally {
        setLoading(false);
      }
    };

    fetchOrdenes();
  }, []);

  const handleSendEvaluation = async () => {
    try {
      // Get the next taskId (mocked here; replace with actual API call if necessary)
      await updateEvaluacion({"caseId": localStorage.getItem("caseId"),
                              "observaciones": observaciones,
                              "cantOrdenesOk": ordenesOk,
                              "cantOrdenesMal": ordenesInv,
                              "cantOrdenes": totalOrdenes,
                              "state": 'FIN'});
        await updateUltimaNotificacion();
      const caseId = localStorage.getItem("caseId");
      if (caseId) {
          const nextTaskId = await getNextTaskId(caseId.toString()); // Replace this with a real nextTaskId API call if needed
          await executeTask(nextTaskId);
          await new Promise(resolve => setTimeout(resolve, 2000));
          const nextTask = await getNextTaskId(caseId);
          const userWalter = await getUsuarioIdByUsername('walter.bates');
          if (userWalter) {
              await assignTask(nextTask, userWalter.toString());
              localStorage.setItem('nextTaskId', nextTask);
          }
          navigate('/paquetes');
      }

    } catch (error) {
      console.error("Error sending evaluation:", error);
      alert("Failed to send evaluation.");
    }
  };

  return (
    <div className="max-w-lg mx-auto p-6 border rounded-lg shadow bg-white">
      <h2 className="text-2xl font-bold text-gray-800 mb-4">Ordenes Evaluation</h2>

      {loading && <p>Loading ordenes...</p>}

      {!loading && ordenes.length > 0 && (
        <form className="space-y-4">
          <div>
            <label className="block text-gray-700 font-medium mb-2">Total Ordenes</label>
            <input
              type="text"
              value={totalOrdenes}
              readOnly
              className="w-full border rounded p-2 bg-gray-100 text-gray-700"
            />
          </div>

          <div>
            <label className="block text-gray-700 font-medium mb-2">Ordenes OK</label>
            <input
              type="text"
              value={ordenesOk}
              readOnly
              className="w-full border rounded p-2 bg-gray-100 text-gray-700"
            />
          </div>

          <div>
            <label className="block text-gray-700 font-medium mb-2">Ordenes INV</label>
            <input
              type="text"
              value={ordenesInv}
              readOnly
              className="w-full border rounded p-2 bg-gray-100 text-gray-700"
            />
          </div>

          <div>
            <label className="block text-gray-700 font-medium mb-2">Observaciones</label>
            <textarea
              value={observaciones}
              onChange={(e) => setObservaciones(e.target.value)}
              className="w-full border rounded p-2 bg-white text-gray-700"
              rows={4}
              placeholder="Add your observations here"
            ></textarea>
          </div>

          <button
            type="button"
            onClick={handleSendEvaluation}
            className="w-full bg-blue-500 text-white py-2 px-4 rounded hover:bg-blue-600"
          >
            Enviar Evaluación
          </button>
        </form>
      )}

      {!loading && ordenes.length === 0 && <p>No ordenes found.</p>}
    </div>
  );
};

export default OrdenesView;