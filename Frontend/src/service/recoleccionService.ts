// src/services/recoleccionService.ts

interface Material {
  id: number;
  nombre: string;
}

interface PuntoRecoleccion {
  id: number;
  nombre: string;
  ubicacion: string;
}

export const addOrden = async (data: any) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Orden/AddOrden`, {  // Usamos la URL desde el .env
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),  // Enviamos los datos del formulario en el body
      });
  
      if (!response.ok) {
        throw new Error('Error al guardar la orden');
      }
  
      return await response.json(); // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a AddOrden:', error);
      throw error;
    }
  };
  
  export const getMateriales = async (): Promise<Material[]> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Material/materiales`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.status !== 200) {
        throw new Error(`Error al obtener los materiales`);
      }
  
      const data: Material[] = await response.json();
      return data; // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a getMateriales:', error);
      throw error;
    }
  };

  export const getPuntosRecoleccion = async (): Promise<PuntoRecoleccion[]> => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/PuntoRecoleccion/PuntosRecoleccion`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.status !== 200) {
        throw new Error(`Error al obtener los puntos de recolección`);
      }
  
      const data: PuntoRecoleccion[] = await response.json();
      return data; // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a getPuntosRecoleccion:', error);
      throw error;
    }
  };

  export const getOrdenesByPaqueteId = async (paqueteId: number) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Orden/ByPaquete/${paqueteId}`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json',
        },
      });

      if (response.status !== 200) {
        throw new Error(`Error al obtener las ordenes`);
      }
  
      const data = await response.json();
      return data; // Retornamos la respuesta si fue exitosa
    } catch (error) {
      console.error('Error en la llamada a getOrdenesByPaqueteId:', error);
      throw error;
    }
  }

  export const updateOrdenState = async (id: number, Estado: string) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_BACKEND_BASE_URL}/Orden/UpdateState/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Estado }),
      });

      if (response.status !== 200) {
        throw new Error(`Error al actualizar la orden: ${response.status}`);
      }
  
      console.log(`Se actualizo la orden: ${id}`);
    } catch (error) {
      console.error('Error en la llamada a updateOrdenState:', error);
      throw error;
    }
  }