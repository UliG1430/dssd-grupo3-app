"""Merge branches

Revision ID: e18eb96cffaf
Revises: 5f06f52b8b77, add_more_orders
Create Date: 2024-10-25 22:31:00.773759

"""
from alembic import op
import sqlalchemy as sa


# revision identifiers, used by Alembic.
revision = 'e18eb96cffaf'
down_revision = ('5f06f52b8b77', 'add_more_orders')
branch_labels = None
depends_on = None


def upgrade():
    pass


def downgrade():
    pass
