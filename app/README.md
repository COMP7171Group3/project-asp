# COMP 7171 Group 3 Project - Kubernetes Deployment

## Overview
ASP.NET Core web application deployed on Kubernetes with Kata Containers for fault tolerance.

## Prerequisites
- **Kubernetes cluster** with Kata runtime support
- **kubectl** configured and connected to your cluster
- **Docker** for building container images
- **SSL certificate** (.pfx file) for HTTPS
- **Persistent storage** support in your cluster

## Deployment Instructions

### Step 1: Verify Prerequisites
```bash
# Check kubectl connectivity
kubectl cluster-info

# Verify Kata runtime support
kubectl get runtimeclass kata

# Check available storage classes
kubectl get storageclass
```

### Step 2: Deploy Infrastructure Components
```bash
# 1. Create Kata RuntimeClass (if not exists)
kubectl apply -f runtimeclass.yaml

# 2. Create Persistent Volume Claim
kubectl apply -f pvc.yaml

# 3. Verify PVC is bound
kubectl get pvc comp7171-db-pvc
```

### Step 3: Create Certificate Secret
```bash
# Replace <path-to-cert> with actual certificate path
# Replace <cert-password> with actual certificate password
kubectl create secret generic comp7171-cert-secret \
  --from-file=aspnetapp.pfx=<path-to-cert> \
  --from-literal=cert-password=<cert-password> \
  --namespace=default
```

### Step 4: Deploy Application
```bash
# Deploy the application
kubectl apply -f deployment.yaml

# Create the service
kubectl apply -f service.yaml

# Monitor deployment progress
kubectl get pods -l app=comp7171-webapp --watch
```

### Step 5: Verify Deployment
```bash
# Check deployment status
kubectl get deployment comp7171-webapp

# Verify all pods are running
kubectl get pods -l app=comp7171-webapp

# Check service external IP
kubectl get service comp7171-service

# Verify Kata containers are being used
kubectl describe pod -l app=comp7171-webapp | grep "Runtime Class"
```

## Access the Application

### Get Service Endpoints
```bash
# Get external IP address
kubectl get service comp7171-service -o wide
```

### Application URLs
- **HTTP**: `http://<EXTERNAL-IP>:8000`
- **HTTPS**: `https://<EXTERNAL-IP>:8001`
- **Health Check**: `http://<EXTERNAL-IP>:8000/health`

### Local Access (if external IP is pending)
```bash
# Port forward for local access
kubectl port-forward service/comp7171-service 8000:8000 8001:8001

# Then access via:
# HTTP:  http://localhost:8000
# HTTPS: https://localhost:8001
```

## Monitoring and Troubleshooting

### View Application Logs
```bash
# View logs from all replicas
kubectl logs -l app=comp7171-webapp --tail=50

# Follow logs in real-time
kubectl logs -l app=comp7171-webapp -f

# View logs from specific pod
kubectl logs <pod-name>
```

### Check Pod Health
```bash
# Describe pods for detailed information
kubectl describe pods -l app=comp7171-webapp

# Check events
kubectl get events --sort-by=.metadata.creationTimestamp
```

### Test Fault Tolerance
```bash
# Delete a pod to test auto-recovery
kubectl delete pod <pod-name>

# Watch new pod being created
kubectl get pods -l app=comp7171-webapp --watch
```

## Development Setup

### Local Development with Docker Compose
```bash
# Start local development environment
docker-compose up -d

# Access application locally
# HTTP:  http://localhost:8000
# HTTPS: https://localhost:8001
```

### Building Custom Images
```bash
# Build container image
docker build -t comp7171-webapp:latest .

# Tag for deployment
docker tag comp7171-webapp:latest comp7171-webapp:v1.0
```

## Cleanup
```bash
# Remove all resources
kubectl delete -f service.yaml
kubectl delete -f deployment.yaml
kubectl delete -f pvc.yaml
kubectl delete secret comp7171-cert-secret
kubectl delete -f runtimeclass.yaml
```

## License
This project is part of COMP 7171 coursework and is for educational purposes.

## References

Kubernetes Templates VS Code extension by Iunuan

Github Copilot
- README generation
- guide for Docker and Kubernetes configurations
- ASP.NET code refactoring aid