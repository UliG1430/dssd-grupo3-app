// src/services/recoleccionService.ts

// src/services/recoleccionService.ts

export interface RecoleccionData {
    material: string;
    quantity: number;
    zone: string; // Agregar el campo "punto"
  }
  
  
  export const enviarRecoleccion = async (data: RecoleccionData): Promise<void> => {
    try {
      const response = await fetch('http://localhost:5000/api/recolecciones', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      });
  
      if (!response.ok) {
        throw new Error('Error al enviar la recolección');
      }
    } catch (error) {
      console.error('Error al conectar con el servidor:', error);
      throw error; // Propaga el error para que pueda ser manejado en el componente
    }
  };
  