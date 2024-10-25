from alembic import op
import sqlalchemy as sa
from sqlalchemy.sql import table, column
from sqlalchemy import Integer, String, Boolean

# revision identifiers, used by Alembic.
revision = 'add_more_orders'  
down_revision = 'b9814b40a536' 
branch_labels = None
depends_on = None

def upgrade():
    orders_table = table('orden',
        column('id', Integer),
        column('punto_recoleccion', String),
        column('peso', Integer),
        column('material', Integer),
        column('id_user', Integer),
        column('reservada', Boolean),
        column('retirada', Boolean)
    )

    # Inserta más órdenes con 'reservada=False'
    op.bulk_insert(orders_table,
        [
            {'punto_recoleccion': 'Point 13', 'peso': 150, 'material': 1, 'id_user': 1, 'reservada': False, 'retirada': False},
            {'punto_recoleccion': 'Point 14', 'peso': 160, 'material': 2, 'id_user': 2, 'reservada': False, 'retirada': False}
        ]
    )

def downgrade():
    pass
