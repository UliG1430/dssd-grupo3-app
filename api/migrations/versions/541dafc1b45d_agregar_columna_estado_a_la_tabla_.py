"""Agregar columna estado a la tabla necesidades y poblar la tabla

Revision ID: 541dafc1b45d
Revises: 37b917eb438e
Create Date: 2024-11-30 07:05:04.445123

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '541dafc1b45d'
down_revision = '37b917eb438e'
branch_labels = None
depends_on = None


def upgrade():
    # Agregar la columna 'estado' a la tabla 'necesidades'
    op.add_column('necesidades', sa.Column('estado', sa.String(length=20), nullable=False, server_default='pendiente'))

    # Poblar la tabla con datos iniciales
    op.execute("""
        INSERT INTO necesidades (material, cod_material, quantity, deposito_id, material_id, estado)
        VALUES 
            ('Madera', 'MAD', 10, 4, 6, 'pendiente'),
            ('Carton', 'CAR', 20, 5, 7, 'pendiente'),
            ('Plastico', 'PLA', 30, 6, 8, 'pendiente');
    """)


def downgrade():
    # Eliminar los datos iniciales (si se necesita revertir)
    op.execute("""
        DELETE FROM necesidades WHERE cod_material IN ('MAD', 'CAR', 'PLA');
    """)

    # Eliminar la columna 'estado'
    op.drop_column('necesidades', 'estado')