export const getPaqueteByCaseId = async (caseId: string) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Paquete/${caseId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.status !== 200) {
        throw new Error(`Error al obtener el paquete`);
      }
  
      const data = await response.json();
      return data; // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a getPaqueteByCaseId:', error);
      throw error;
    }
  }

export const updatePaquete = async (id: number, state: string) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Paquete/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ state }),
      });

      if (response.status !== 200) {
        throw new Error(`Error al actualizar el paquete: ${response.status}`);
      }
  
      console.log(`Se actualizo el paquete: ${id}`);
    } catch (error) {
      console.error('Error en la llamada a updatePaquete:', error);
      throw error;
    }
  }

  export const addPaquete = async (caseId: number, state: string) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Paquete/AddPaquete`, {  // Usamos la URL desde el .env
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({caseId, state}),  // Enviamos los datos del formulario en el body
      });
  
      if (!response.ok) {
        throw new Error('Error al guardar el paquete');
      }
  
      return await response.json(); // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a AddPaquete:', error);
      throw error;
    }
  };

  export const getPaquetesByState = async (state: string) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Paquete/byState/${state}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.status !== 200) {
        throw new Error(`Error al obtener los paquetes`);
      }
  
      const data = await response.json();
      return data; // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a getPaquetesByState:', error);
      throw error;
    }
  }