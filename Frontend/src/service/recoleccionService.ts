// src/services/recoleccionService.ts
export const enviarRecoleccion = async (data: any) => {
    const response = await fetch('/api/recoleccion', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
  
    if (!response.ok) {
      throw new Error('Error enviando los datos de recolección');
    }
  
    return response.json();
  };
  