from app import db

class Material(db.Model):
    __tablename__ = 'materiales'

    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    name = db.Column(db.String(100), nullable=False)
    cod_material = db.Column(db.String(3), nullable=True)  # Nueva columna