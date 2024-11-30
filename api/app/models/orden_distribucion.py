from app import db

class OrdenDistribucion(db.Model):
    __tablename__ = 'ordenes_distribucion'

    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    necesidad_id = db.Column(db.Integer, db.ForeignKey('necesidades.id'), nullable=False)
    estado = db.Column(db.String(20), nullable=False, default='pendiente')

    # Relación con la tabla `Necesidad`
    necesidad = db.relationship('Necesidad', backref='ordenes_distribucion')

    def __repr__(self):
        return f'<OrdenDistribucion necesidad_id={self.necesidad_id} estado={self.estado}>'