import React from 'react';
import { Box, Typography } from '@mui/material';

const EsperarCobro: React.FC = () => {
    return (
        <Box
            display="flex"
            flexDirection="column"
            justifyContent="center"
            alignItems="center"
            height="100vh"
            textAlign="center"
        >
            <Typography variant="h4" gutterBottom>
                Espere a recibir la notificación de cobro
            </Typography>
            <Typography variant="subtitle1" color="textSecondary">
                Esto puede tardar algunos días
            </Typography>
        </Box>
    );
};

export default EsperarCobro;