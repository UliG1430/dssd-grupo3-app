"""Agregar columna rol a users y asignar roles

Revision ID: 258dd7e9993e
Revises: eae21f7bda57
Create Date: 2024-11-30 10:06:55.775455

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = '258dd7e9993e'
down_revision = 'eae21f7bda57'
branch_labels = None
depends_on = None


def upgrade():
    # Agregar la columna 'rol' a la tabla 'users'
    op.add_column('users', sa.Column('rol', sa.String(length=20), nullable=False, server_default='user'))

    # Asignar roles a los usuarios existentes
    op.execute("UPDATE users SET rol = 'A' WHERE username = 'william.jobs'")
    op.execute("UPDATE users SET rol = 'D' WHERE username = 'thomas.wallis'")


def downgrade():
    # Eliminar la columna 'rol' de la tabla 'users'
    op.drop_column('users', 'rol')
