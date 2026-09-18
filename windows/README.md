# HyS Control de Trabajos — Windows

Versión de escritorio para Windows del sistema HyS Control de Trabajos.

## Objetivo

La aplicación de Windows compartirá los mismos trabajos con la aplicación móvil mediante una base de datos en la nube.

## Funciones previstas

- Ver trabajos pendientes y terminados.
- Registrar y editar trabajos.
- Área de trabajo como dato principal.
- Descripción enriquecida de trabajo realizado.
- Importe.
- Cambio de estado pendiente/terminado.
- Sincronización con la app Android.
- Generación de documentos para facturación.
- Interfaz adaptada a escritorio Windows.

## Arquitectura

La carpeta `windows` contiene el cliente de escritorio. La sincronización se implementará mediante Supabase para evitar mantener un servidor propio y permitir acceso desde teléfono y PC.

> La aplicación Android actual debe seguir funcionando durante la migración. La sincronización se incorporará de forma gradual para no perder los datos locales existentes.
