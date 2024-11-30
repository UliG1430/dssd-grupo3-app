"""Insertar datos en tablas de usuarios, materiales y depositos

Revision ID: 6a2f7925fb1b
Revises: None  # Esto indica que no depende de ninguna migración anterior
Create Date: 2024-11-30 05:33:09.634336
"""

from alembic import op
import sqlalchemy as sa

# Agregar el identificador de la migración y su relación con la anterior
revision = '6a2f7925fb1b'  # El ID único de esta migración
down_revision = None  # Es 'None' porque no tiene una migración anterior (primera migración)
branch_labels = None
depends_on = None

def upgrade():
    # Insertar usuarios con contraseñas en texto plano
    op.execute("INSERT INTO users (username, mail, password) VALUES ('william.jobs', 'william.jobs@example.com', 'bpm')")
    op.execute("INSERT INTO users (username, mail, password) VALUES ('thomas.wallis', 'thomas.wallis@example.com', 'bpm')")

    # Insertar materiales
    op.execute("INSERT INTO materiales (name, cod_material) VALUES ('Madera', 'MAD')")
    op.execute("INSERT INTO materiales (name, cod_material) VALUES ('Carton', 'CAR')")
    op.execute("INSERT INTO materiales (name, cod_material) VALUES ('Plastico', 'PLA')")
    op.execute("INSERT INTO materiales (name, cod_material) VALUES ('Vidrio', 'VID')")
    op.execute("INSERT INTO materiales (name, cod_material) VALUES ('Metal', 'MET')")

    # Insertar depósitos
    op.execute("INSERT INTO depositos (name) VALUES ('Deposito 1')")
    op.execute("INSERT INTO depositos (name) VALUES ('Deposito 2')")
    op.execute("INSERT INTO depositos (name) VALUES ('Deposito 3')")
def downgrade():
    # Opcional: Eliminar los datos si necesitas revertir la migración
    op.execute("DELETE FROM users")
    op.execute("DELETE FROM materiales")
    op.execute("DELETE FROM depositos")