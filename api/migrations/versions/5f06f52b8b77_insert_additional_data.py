"""Insert additional data

Revision ID: 5f06f52b8b77
Revises: b9814b40a536
Create Date: 2024-10-24 23:21:50.139684

"""
from alembic import op
import sqlalchemy as sa
from sqlalchemy.sql import table, column
from sqlalchemy import Integer, String
from werkzeug.security import generate_password_hash
# revision identifiers, used by Alembic.
revision = '5f06f52b8b77'
down_revision = 'b9814b40a536'
branch_labels = None
depends_on = None


def upgrade():
    # Insertar más datos en la tabla 'depositos'
    depositos_table = sa.table('depositos',
        column('id', Integer),
        column('name', String)
    )
    op.bulk_insert(depositos_table,
        [
            {'id': 1, 'name': 'Depósito Central'},
            {'id': 2, 'name': 'Depósito Norte'},
            {'id': 3, 'name': 'Depósito Sur'}
        ]
    )

    # Insertar más datos en la tabla 'materiales'
    materiales_table = table('materiales',
        column('id', Integer),
        column('name', String)
    )
    op.bulk_insert(materiales_table,
        [
            {'id': 1, 'name': 'Material A'},
            {'id': 2, 'name': 'Material B'},
            {'id': 3, 'name': 'Material C'}
        ]
    )

    # Insertar relaciones en la tabla 'deposito_proveedor'
    deposito_proveedor_table = table('deposito_proveedor',
        column('id', Integer),
        column('deposito_id', Integer),
        column('material_id', Integer)
    )
    op.bulk_insert(deposito_proveedor_table,
        [
            {'id': 1, 'deposito_id': 1, 'material_id': 1},
            {'id': 2, 'deposito_id': 2, 'material_id': 2},
            {'id': 3, 'deposito_id': 3, 'material_id': 3}
        ]
    )

    # Insertar más usuarios
    users_table = table('users',
        column('mail', String),
        column('username', String),
        column('password', String)
    )

    # Insertar usuarios con contraseñas hasheadas
    op.bulk_insert(users_table,
        [
            {'mail': 'user1@example.com', 'username': 'user1', 'password': generate_password_hash('password1')},
            {'mail': 'user2@example.com', 'username': 'user2', 'password': generate_password_hash('password2')},
            {'mail': 'user3@example.com', 'username': 'user3', 'password': generate_password_hash('password3')}
        ]
    )

    # Insertar órdenes en la tabla 'orden'
    orden_table = table('orden',
        column('punto_recoleccion', String),
        column('peso', Integer),
        column('material', Integer),
        column('id_user', Integer),
        column('reservada', sa.Boolean),
        column('retirada', sa.Boolean)
    )
    op.bulk_insert(orden_table,
        [
            {'punto_recoleccion': 'Point 1', 'peso': 100, 'material': 1, 'id_user': 1, 'reservada': True, 'retirada': False},
            {'punto_recoleccion': 'Point 2', 'peso': 200, 'material': 2, 'id_user': 2, 'reservada': False, 'retirada': True},
            {'punto_recoleccion': 'Point 3', 'peso': 150, 'material': 3, 'id_user': 3, 'reservada': True, 'retirada': True}
        ]
    )

def downgrade():
    # Aquí puedes implementar lógica para eliminar los datos si lo deseas
    pass