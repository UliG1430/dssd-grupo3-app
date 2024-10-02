// src/services/recoleccionService.ts

export const addOrden = async (data: any) => {
    try {
      const response = await fetch('/api/orden/add', {  
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
  