

export const getNotificacionPagoByCaseId = async (caseId: string) => {
    try {
        const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/NotificacionPago/ByCaseId/${caseId}`, {
            method: 'GET',
            headers: {
            'Content-Type': 'application/json',
            },
        });
        if (response.status != 204)
            return await response.json();
        else
            return null // Retornamos la respuesta si fue exitosa
    } catch (error) {
        console.error('Error retrieving notificacion pago by case id:', error);
        throw error;
    }
};

export const addNotificacionPago = async (notificacionPago: any) => {
    try {
        const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/NotificacionPago`, {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            },
            body: JSON.stringify( notificacionPago )
        });
            
        if (response.status !== 200) {
            throw new Error(`Error al obtener la notificacion pago`);
        }

        const data = await response.json();
        return data; // Retornamos la respuesta si fue exitosa
    } catch (error) {
        console.error('Error adding notificacion pago:', error);
        throw error;
    }
};
