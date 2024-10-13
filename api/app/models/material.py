from app import db

class Material(db.Model):
    __tablename__ = 'materiales'

    id = db.Column(db.Integer, primary_key=True, autoincrement=True)
    name = db.Column(db.String(100), nullable=False)

    def __repr__(self):
        return f"<Material {self.name}>"