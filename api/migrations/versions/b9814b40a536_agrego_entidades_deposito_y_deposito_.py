from alembic import op
import sqlalchemy as sa
from sqlalchemy.sql import table, column
from sqlalchemy import String
from werkzeug.security import generate_password_hash  # Para hashear la contraseña

# revision identifiers, used by Alembic.
revision = 'b9814b40a536'
down_revision = 'c9d6e0ce7479'
branch_labels = None
depends_on = None

def upgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    # Verificar si la tabla 'depositos' ya existe antes de crearla
    if not op.get_bind().dialect.has_table(op.get_bind(), 'depositos'):
        op.create_table('depositos',
            sa.Column('id', sa.Integer(), autoincrement=True, nullable=False),
            sa.Column('name', sa.String(length=255), nullable=False),
            sa.PrimaryKeyConstraint('id')
        )

    # Verificar si la tabla 'deposito_proveedor' ya existe antes de crearla
    if not op.get_bind().dialect.has_table(op.get_bind(), 'deposito_proveedor'):
        op.create_table('deposito_proveedor',
            sa.Column('id', sa.Integer(), autoincrement=True, nullable=False),
            sa.Column('deposito_id', sa.Integer(), nullable=False),
            sa.Column('material_id', sa.Integer(), nullable=False),
            sa.ForeignKeyConstraint(['deposito_id'], ['depositos.id']),
            sa.ForeignKeyConstraint(['material_id'], ['materiales.id']),
            sa.PrimaryKeyConstraint('id')
        )

    # Agregar usuario predeterminado a la tabla users
    users_table = table('users',
        column('mail', String),
        column('username', String),
        column('password', String)
    )

    # Hashear la contraseña antes de insertarla
    hashed_password = generate_password_hash('admin')

    # Insertar el usuario admin
    op.bulk_insert(users_table,
        [{'mail': 'admin@example.com', 'username': 'admin', 'password': hashed_password}]
    )
    # ### end Alembic commands ###

def downgrade():
    # ### commands auto generated by Alembic - please adjust! ###
    op.drop_table('deposito_proveedor')
    op.drop_table('depositos')
    # ### end Alembic commands ###
